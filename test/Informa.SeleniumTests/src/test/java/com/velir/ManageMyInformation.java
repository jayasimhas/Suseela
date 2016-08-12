package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.List;
import java.util.UUID;

/**
 * Created by Ishan on 8/2/2016.
 */
public class ManageMyInformation extends SetupClass {

    By title =By.cssSelector(".page-account__header");
    By subheader = By.cssSelector(".page-account__subheader");

    @Test
    public void pageAccessibility(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID")+"@yopmail.com",configuration.getString("password"));

        helper.getURL(ENV+"my-account/contact-information");

//        helper.click(By.linkText("My Account"));
//
//        helper.click(By.linkText("ACCOUNT SETTINGS"));

        Assert.assertEquals(helper.getElementText(title), "Account Settings");

    }

    @Test
    public void availableSections(){

        helper.getURL(ENV);

        List<String> subheaderExpected = Arrays.asList("User Name / Email Address", "Update Your Password","Name","Company & Job Information","Phone & Fax","Billing Address","Shipping Address");

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID")+"@yopmail.com",configuration.getString("password"));

        helper.getURL(ENV+"my-account/contact-information");

//        helper.click(By.linkText("My Account"));
//
//        helper.click(By.linkText("ACCOUNT SETTINGS"));

        Assert.assertEquals(helper.getElementsText(subheader), subheaderExpected);

    }

    @Test
    public void updatePassword(){

        helper.getURL(ENV);


        HomePage homePage = new HomePage(driver);
//
//        homePage.loginProcess(configuration.getString("emailUpdate") + "@yopmail.com", configuration.getString("passwordOld"));


        String uuid = UUID.randomUUID().toString();
        helper.log("uuid = " + uuid);


        clickRegisterNEnterEmail(uuid + "@yopmail.com");


        registerForAccount();

        registerPageTwo();

        helper.getURL(ENV);

        passwordUpdater(configuration.getString("password"), configuration.getString("passwordNew"));

        helper.waitForSeconds(2);

        Assert.assertEquals(helper.getElementText(By.xpath("//div[2]/form/div[1]/p")), "Password Updated");

        // Find an element
//        WebElement elementToClick = driver.findElement(By.xpath("//div[3]/span/a"));
//        ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + elementToClick.getLocation().y + ")");
//
//        helper.waitForSeconds(3);
//        elementToClick.click();

        helper.clickLocation(By.xpath("//div[3]/span/a"),"y");

        homePage.loginProcess(uuid + "@yopmail.com", configuration.getString("passwordNew"));

       // passwordUpdater(configuration.getString("passwordNew"), configuration.getString("passwordOld"));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".header-account-access__label>a")), "My Account");


    }

    private void passwordUpdater(String passwordOld, String passwordNew) {
        helper.getURL(ENV + "my-account/contact-information");
        helper.sendKeys(By.name("CurrentPassword"), passwordOld);
        helper.sendKeys(By.name("NewPassword"), passwordNew);
        helper.sendKeys(By.name("NewPasswordConfirm"), passwordNew);


        helper.waitForSeconds(2);

        if (BROWSER.equalsIgnoreCase("Mobile"))
            helper.clickLocation(By.xpath("//form/div[6]/button"),"x");
        else
            helper.click(By.cssSelector(".page-preferences__submit-wrapper>button"));


        helper.waitForSeconds(2);
    }



    @Test
    public void updateContactDetails(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));

        helper.getURL(ENV + "my-account/contact-information");


        helper.sendKeys(By.id("txtShippingCity"), UUID.randomUUID().toString());


        By updateLocator= By.cssSelector(".manage-preferences");

        helper.waitForSeconds(2);
            helper.click(updateLocator);


        Assert.assertEquals(helper.getElementText(By.xpath("//div[2]/div[4]/p")), "Account Updated");


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
            helper.waitForSeconds(4);
            ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().x + ")");}
        else
            ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().y + ")");


        helper.waitForSeconds(3);
        clickReg.click();



        helper.click(By.cssSelector(".button--filled.js-register-user-submit"));

        helper.waitForSeconds(10);
    }

    private void clickRegisterNEnterEmail(String user) {
        By username =By.xpath("//div[4]/div[2]/form/input"); ;
        helper.click(By.xpath("//div[4]/div[5]/div[1]"));

        helper.waitForSeconds(2);
        driver.findElement(username).sendKeys(user);

        helper.click(By.xpath("//div[4]/div[2]/form/button"));
    }
}
