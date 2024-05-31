package com.hnnuodou;


import org.apache.log4j.LogManager;
import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;

import java.io.*;
import java.util.Properties;

public class Util {
   static Logger logger=LogManager.getLogger(Util.class);
    static String configFileName = System.getProperty("user.dir") + File.separator;

    public static Properties readProperties() {
      Properties resProperties=readPropertiesFromResource();
      Properties fileProperties=readPropertiesFromFile();
        for(String key :fileProperties.stringPropertyNames()){
            resProperties.setProperty(key,fileProperties.getProperty(key));
        }
        return  resProperties;
    }

    public static Properties readPropertiesFromResource(){
        ClassLoader classLoader = Main.class.getClassLoader();
        Properties properties = new Properties();
        InputStream inputStream = null;
        try {

                inputStream = classLoader.getResourceAsStream("config.properties");

            properties.load(inputStream);
        } catch (IOException e) {
            logger.error("读取配置文件异常发生",e);
            e.printStackTrace();
        } finally {
            try {
                if (inputStream != null)
                    inputStream.close();
            } catch (IOException e) {
                logger.error("读取配置文件异常发生",e);
            }
        }
        return properties;
    }
    public  static  Properties readPropertiesFromFile(){
        Properties properties = new Properties();
        InputStream inputStream = null;
        try {
            File file = new File(configFileName + "config.properties");
            if (file.exists()) {
                inputStream = new BufferedInputStream(new FileInputStream(file));
                properties.load(inputStream);
            }
        } catch (FileNotFoundException e) {
            logger.error("读取配置文件异常发生",e);
        } catch (IOException e) {
            logger.error("读取配置文件异常发生",e);
        } finally {
            try {
                if (inputStream != null)
                    inputStream.close();
            } catch (IOException e) {
                logger.error("读取配置文件异常发生",e);
            }
        }
        return properties;
    }

    public static void readLog4jConfig() throws IOException {

        ClassLoader classLoader = Main.class.getClassLoader();
        InputStream inputStream = null;
        try {
            File file = new File(configFileName + "log4j.properties");
            if (file.exists()) {
                inputStream = new BufferedInputStream(new FileInputStream(file));
            } else {
                inputStream = classLoader.getResourceAsStream("log4j.properties");
            }
            PropertyConfigurator.configure(inputStream);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                if (inputStream != null)
                    inputStream.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }
}
