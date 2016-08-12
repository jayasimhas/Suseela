package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.UUID;

/**
 * Created by ishan.kumar on 7/18/2016.
 */
public class Registration extends SetupClass {


    @Test
    public void registrationLink(){


        String uuid = UUID.randomUUID().toString();
        helper.log("uuid = " + uuid);

        By username =By.xpath("//div[4]/div[2]/form/input");
        helper.getURL(ENV);
        clickRegisterNEnterEmail("qaTestInforma");

        Assert.assertEquals(helper.getElementText(By.xpath("//div[4]/div[2]/form/p[1]")), "Your username does not meet the requirements.");

        driver.findElement(username).clear();
        driver.findElement(username).sendKeys(uuid + "@yopmail.com");

        helper.click(By.xpath("//div[2]/form/button"));

        helper.waitForSeconds(6);

        Assert.assertEquals(helper.getElementText(By.cssSelector(".page-registration__header")),"Register for an Account");



    }

    @Test
    public void accountPage(){


        String uuid = UUID.randomUUID().toString();
        helper.log("uuid = " + uuid);

       // By username =By.xpath("//div[4]/div[2]/form/input");
        helper.getURL(ENV);
        clickRegisterNEnterEmail(uuid + "@yopmail.com");


        registerForAccount();

        helper.waitForSeconds(5);
        Assert.assertEquals(helper.getElementText(By.cssSelector(".page-registration__header")), "You're Almost Done");



    }

    @Test
    public void registerStepTwo(){


        String uuid = UUID.randomUUID().toString();
        helper.log("uuid = " + uuid);


        helper.getURL(ENV);
        clickRegisterNEnterEmail(uuid + "@yopmail.com");


        registerForAccount();

        registerPageTwo();
        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");



    }



    @Test
    public void RedirectAfterRegister(){


        String uuid = UUID.randomUUID().toString();
        helper.log("uuid = " + uuid);


        helper.getURL(ENV + "/search#?q=test");

        clickRegisterNEnterEmail(uuid + "@yopmail.com");


        registerForAccount();

        registerPageTwo();
       // Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");


        Assert.assertEquals(helper.getElementText(By.cssSelector(".search__header-title")), "Search Results");

    }

    private void registerPageTwo() {
        helper.waitForSeconds(2);
        helper.click(By.cssSelector(".button--filled.registration-final.js-register-final"));


        helper.waitTillElementLocated(By.cssSelector(".header-account-access__label>a"));
    }

    private void registerForAccount() {
        helper.waitTillElementLocated(By.cssSelector(".page-registration__header"));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".page-registration__header")), "Register for an Account");


        driver.findElement(By.name("firstName")).sendKeys("Quality");

       // helper.waitForSeconds(2);

        driver.findElement(By.name("lastName")).sendKeys("Automation");

       // helper.waitForSeconds(2);

        driver.findElement(By.name("password")).sendKeys("!@#Velir321");

       // helper.waitForSeconds(2);

        driver.findElement(By.name("passwordRepeat")).sendKeys("!@#Velir321");

        helper.waitForSeconds(4);

        WebElement clickReg = driver.findElement(By.name("termsAccepted"));

       // helper.log(clickReg.getLocation());
//        Actions builder = new Actions(driver);
//        builder.moveToElement(clickReg, 90, 1070).click().build().perform();


        if (BROWSER.equalsIgnoreCase("Mobile")){
            helper.waitForSeconds(2);
        ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().x + ")");}
        else
            ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().y + ")");


        helper.waitForSeconds(3);
        clickReg.click();

        //helper.waitForSeconds(2);

        helper.click(By.cssSelector(".button--filled.js-register-user-submit"));

        helper.waitForSeconds(10);
    }

    private void clickRegisterNEnterEmail(String user) {
        By username =By.xpath("//div[4]/div[2]/form/input"); ;
        helper.click(By.xpath("//div[4]/div[5]/div[1]"));

        helper.waitForSeconds(2);
        helper.sendKeys(username, user);

       // helper.click(By.xpath("//div[4]/div[2]/form/button"));

        helper.click(By.cssSelector(".js-pre-register-user-submit"));
    }

}
