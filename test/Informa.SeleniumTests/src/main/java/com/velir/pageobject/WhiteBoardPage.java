package com.velir.pageobject;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

/**
 * Created by ishan.kumar on 3/25/2016.
 */
public class WhiteBoardPage extends GeneralPage {

    //public WebDriver driver;

    public WhiteBoardPage(WebDriver driver) {
        super(driver);
    }

//    public WhiteBoardPage(WebDriver driver) {
//       this.driver=driver;
//    }


    public void boardLogin(String username,String password) {


        helper.waitTillElementLocated(By.id("UserName"));
        helper.sendKeys(By.id("UserName"), username);
        helper.sendKeys(By.id("Password"),password);
        helper.click(By.name("ctl07"));
        helper.waitTillElementLocated(By.id("btnLogout"));

    }





}
