package com.hnnuodou;


import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;
import redis.clients.jedis.JedisPoolConfig;

import java.util.Properties;

public class RedisClient {

    private static Logger log = LoggerFactory.getLogger(RedisClient.class);
    private String host = "localhost";
    private Integer port = 6379;
    private String password;
    private String queueName;
    private Integer maxActive;
    private Integer maxRetryNumber = 3;
    private JedisPool jedisPool;
    private Properties properties;


    public RedisClient(Properties properties) {
        this.properties = properties;
        this.host = properties.getProperty("redis.host");
        this.port = Integer.valueOf(properties.getProperty("redis.port"));
        this.password = properties.getProperty("redis.password");
        this.queueName = properties.getProperty("redis.queueName");
        this.maxActive = Integer.valueOf(properties.getProperty("redis.maxActive"));
        this.init();
    }

    private void init() {

        JedisPoolConfig config = new JedisPoolConfig();
        config.setMaxTotal(maxActive);
        config.setMaxIdle(5);
        config.setMaxWaitMillis(1000 * 100);
        config.setTestOnBorrow(true);
        if (!StringUtils.isEmpty(password)) {
            jedisPool = new JedisPool(config, host, port, 10 * 1000, password);
        } else {
            jedisPool = new JedisPool(config, host, port);
        }
    }

    /**
     * 推送消息到redis队列
     *
     * @param message
     */
    public void RPush(String message) {
        log.info(message);
        RPushRetry(message, 1);
    }

    private void RPushRetry(String message, int number) {
        if (number >= maxRetryNumber) {
            return;
        }
        number++;
        Jedis jedis = null;
        try {
            jedis = jedisPool.getResource();
            Long rpushRst = jedis.rpush(queueName, message);
            log.info("Redis写入结果：" + rpushRst);
            return;
        } catch (Exception ex) {
            log.error("第" + number + "次尝试,Redis写入数据异常" + message, ex);
        } finally {
            if (jedis != null) {
                jedis.close();
            }
        }
        RPushRetry(message, number);
    }


}
