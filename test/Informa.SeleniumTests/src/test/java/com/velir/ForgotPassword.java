package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 4/22/2016.
 */
public class ForgotPassword extends SetupClass {



    @Test
    public void loginFormDisplay(){

        helper.getURL(ENV);


        HomePage homePage =new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID"), configuration.getString("password"));


        String invalid = driver.findElement(By.xpath("//form/div")).getText();

        Assert.assertEquals(invalid, "Your login and/or password information does not match our records. Please try again.");

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".dismiss-button")));

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".pop-out__sign-in-remember-me>input")));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".js-show-forgot-password")), "Forgot your password?");



    }

    @Test
    public void frontendLogin(){

        helper.getURL(ENV);



        HomePage homePage =new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));


        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

        helper.waitForSeconds(2);
        helper.click(By.xpath("//div[3]/span"));


        helper.waitForSeconds(4);


        homePage.loginProcess(configuration.getString("emailID"), configuration.getString("password"));

        helper.waitForSeconds(2);

        String invalid = driver.findElement(By.xpath("//form/div")).getText();

        Assert.assertEquals(invalid, "Your login and/or password information does not match our records. Please try again.");



    }

    //Only run for Scrip
    @Test
    public void singleSignOn() {

        helper.getURL(ENV);

        HomePage homePage =new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));


        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

        helper.getURL(configuration.getString("Stage.Pink"));



        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

        helper.getURL(configuration.getString("Stage.Medtech"));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

        helper.getURL(configuration.getString("Stage.Rose"));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

        helper.getURL(configuration.getString("Stage.InVivoDev"));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");

    }





    //@Test
    public void invalidEmail(){

        helper.getURL(ENV);

        forgotPasswordEnter("invalid");


        //helper.log(driver.findElement(By.xpath("//form/p")).getText());
        //Assert.assertEquals(driver.findElement(By.xpath("//form/p")).getText(), "Need help? Contact us at (800) 332-2181, +1 (908) 748-1221, or custcare@informa.com");

        Assert.assertEquals(driver.findElement(By.xpath("//p[3]")).getText(), "Please enter a valid email address.");

    }


    //@Test
    public void forgotPasswordLink() {

        helper.getURL(ENV);

        forgotPasswordEnter(configuration.getString("emailID")+"@yopmail.com");


        By messageLocator = By.cssSelector("div.alert-success.js-reset-password-success > p");
        helper.waitTillElementLocated(messageLocator);
        //helper.log(driver.findElement(By.xpath("//form/p")).getText());
        //Assert.assertEquals(driver.findElement(By.xpath("//form/p")).getText(), "Need help? Contact us at (800) 332-2181, +1 (908) 748-1221, or custcare@informa.com");

        String successMessage = driver.findElement(messageLocator).getText();

        Assert.assertEquals(successMessage, "Thanks, we've sent a link to reset your password");

        helper.getURL("http://yopmail.com/ishankumarinforma");

        Assert.assertTrue(helper.getElementText(By.xpath("//tr/td/p[2]/strong")).contains("Dear Ishan Kumar,\n" +
                "\n" +
                "You have requested to change the password"));





    }


    private void forgotPasswordEnter(String s) {
        helper.click(By.xpath("//div[2]/span"));
        helper.click(By.xpath("//form/div[2]/div/span"));
        driver.findElement(By.name("email")).clear();
        driver.findElement(By.name("email")).sendKeys(s);
        helper.click(By.xpath("(//button[@type='submit'])[2]"));
    }


}
