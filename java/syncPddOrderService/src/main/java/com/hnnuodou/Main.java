package com.hnnuodou;


import com.pdd.pop.sdk.common.util.JsonUtil;
import com.pdd.pop.sdk.message.MessageHandler;
import com.pdd.pop.sdk.message.WsClient;
import com.pdd.pop.sdk.message.model.Message;
import org.apache.commons.lang3.StringUtils;
import org.apache.log4j.Logger;

import java.io.IOException;
import java.util.Properties;


public class Main {
    static Properties properties;
    static WsClient wsClient;
    static Logger logger = Logger.getLogger(Main.class);
    static RedisClient redisClient;

    public static void main(String[] args) throws IOException {

        Util.readLog4jConfig();
        properties = Util.readProperties();
        redisClient = new RedisClient(properties);
        startPddWsSocket();


    }

    private static void startPddWsSocket() {
        String wsAddress = properties.getProperty("pdd.wsAddress");
        String clientId = properties.getProperty("pdd.clientId");
        String clientSecret = properties.getProperty("pdd.clientSecret");

        if (StringUtils.isEmpty(wsAddress)) {
            printError("wsAddress为空");
        }
        if (StringUtils.isEmpty(clientId)) {
            printError("clientId为空");
        }
        if (StringUtils.isEmpty(clientSecret)) {
            printError("clientSecret为空");
        }
        wsClient = new WsClient(
                wsAddress, clientId,
                clientSecret, new MessageHandler() {
            @Override
            public void onMessage(Message message) throws Exception {
                try {
                    String messageBody = JsonUtil.transferToJson(message);
                    Thread.sleep(500);
                    logger.info("收到从拼多多推送的消息：" + messageBody);
                    redisClient.RPush(messageBody);

                } catch (Exception e) {
                    logger.error("未知错误:" + e.getMessage(), e);
                }
                //业务处理
            }
        });
        wsClient.connect();
        logger.debug("拼多多客户端启动成功");
    }

    private static void printError(String message) {
        logger.info(message);
        System.exit(-1);
    }

}
