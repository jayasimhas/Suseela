package com.velir.baseclass;

import com.velir.utilities.Helper;
import org.apache.commons.configuration.PropertiesConfiguration;
import org.openqa.selenium.Dimension;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.testng.annotations.*;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.HashMap;
import java.util.Map;


public class SetupClass {
    protected WebDriver driver;
    public static final String USERNAME = "ishankumar2";
    public static final String AUTOMATE_KEY = "RrUSaV31kAQd3gXxq6Wy";
    public static final String URLBrowserStack = "http://" + USERNAME + ":" + AUTOMATE_KEY + "@hub.browserstack.com/wd/hub";
    public Helper helper;
    public String ENV;
    public String BROWSER;
    public String ENV_BE;
    public PropertiesConfiguration configuration ;
    protected PropertiesConfiguration expectedData;

    @BeforeMethod
    @Parameters({"browser","environment"})
    public void launchBrowser(@Optional("Mobile")String browser,@Optional("Stage.Scrip") String environment){  //to launch browser, open url and click on our consultants

            switch (browser){
                case "Chrome":
                    System.setProperty("webdriver.chrome.driver",
                            "C:\\Browser Drivers\\chromedriver.exe");
                    driver = new ChromeDriver();
                    driver.manage().window().setSize(new Dimension(1600, 1200));
                    break;

                case "FF":
                    driver = new FirefoxDriver();
                    //driver.manage().window().setSize(new Dimension(1200, 1000));
                    driver.manage().window().setSize(new Dimension(1600, 1200));
                    break;

                case "IE":
                    System.setProperty("webdriver.ie.driver",
                            "C:\\Browser Drivers\\IEDriverServer.exe");
                    driver = new InternetExplorerDriver();
                    //driver.manage().window().maximize();
                    driver.manage().window().setSize(new Dimension(1600, 1200));
                    break;


                case "Mobile":
                    System.setProperty("webdriver.chrome.driver",
                            "C:\\Browser Drivers\\chromedriver.exe");
                    Map<String, String> mobileEmulation = new HashMap<String, String>();
                   //mobileEmulation.put("deviceName", "Apple iPhone 6");
                    mobileEmulation.put("deviceName", "Google Nexus 6");




                    Map<String, Object> chromeOptions = new HashMap<String, Object>();
                    chromeOptions.put("mobileEmulation", mobileEmulation);
                    DesiredCapabilities capabilities = DesiredCapabilities.chrome();
                    capabilities.setCapability(ChromeOptions.CAPABILITY, chromeOptions);
                    driver = new ChromeDriver(capabilities);
                    //driver.manage().window().setSize(new Dimension(600, 1400));
                    break;

                case "iPad":
                    System.setProperty("webdriver.chrome.driver",
                            "C:\\Browser Drivers\\chromedriver.exe");
                    mobileEmulation = new HashMap<String, String>();
                    mobileEmulation.put("deviceName", "Apple iPad");

                    chromeOptions = new HashMap<String, Object>();
                    chromeOptions.put("mobileEmulation", mobileEmulation);
                    capabilities = DesiredCapabilities.safari();
                    capabilities.setCapability(ChromeOptions.CAPABILITY, chromeOptions);
                    driver = new ChromeDriver(capabilities);
                   // driver.manage().window().setSize(new Dimension(1600, 1200));

                    break;

                default:
                    driver = new FirefoxDriver();
                    driver.manage().window().setSize(new Dimension(1600, 1200));
                    break;
            }
        helper =new Helper(driver);
        try {
            configuration = new PropertiesConfiguration("testdata/config.properties");
            expectedData = new PropertiesConfiguration("testdata/expecteddata.properties");
            ENV = configuration.getString(environment);
            BROWSER=browser;
            ENV_BE = configuration.getString("Stage.Auth");
        }catch (Exception E){

        }
        }



    @AfterMethod
    public void closeBrowser(){ //to close and quit browser
        driver.close();
        driver.quit();
    }
}
