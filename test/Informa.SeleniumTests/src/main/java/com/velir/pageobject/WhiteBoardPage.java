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


    public void boardLogin() {


        helper.waitTillElementLocated(By.id("UserName"));
        driver.findElement(By.id("UserName")).clear();
        driver.findElement(By.id("UserName")).sendKeys("admin");
        driver.findElement(By.id("Password")).clear();
        driver.findElement(By.id("Password")).sendKeys("Velir123!");
        driver.findElement(By.name("ctl07")).click();
        helper.waitTillElementLocated(By.id("btnLogout"));
        //assertEquals(driver.findElement(By.id("btnLogout")).getAttribute("value"), "Logout");
    }





}
