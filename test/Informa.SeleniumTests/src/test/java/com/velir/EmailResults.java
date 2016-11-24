package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by ishan.kumar on 5/23/2016.
 */
public class EmailResults extends SetupClass {

    By sendLocator = By.cssSelector("form.form-email-search > div.pop-out__form-actions-wrapper > div.sign-in__submit-wrapper > button.button--filled.click-utag");



    @Test
    public void componentFields(){

        helper.getURL(ENV + "/search#?q=test");

        By cancelLocator = By.cssSelector("form.form-email-search > div.pop-out__form-actions-wrapper > div.dismiss-button > span.highlight-link");


        fillForm("informaAutomation@yopmail.com");

        String send = driver.findElement(sendLocator).getText();
        Assert.assertEquals(send, "SEND");


        if (BROWSER.equalsIgnoreCase("Mobile")){
            helper.getURL(ENV + "/search#?q=test");
            helper.waitForSeconds(2);
             }
        else
            driver.findElement(cancelLocator).click();


        helper.waitForSeconds(2);
        Assert.assertFalse(driver.findElement(sendLocator).isDisplayed());





    }

    private void fillForm(String email) {
        By emailIcon =By.xpath("//header/div[1]/ul/li[1]/a");

        By senderName =By.name("SenderName");
        helper.click(emailIcon);

        helper.sendKeys(By.name("RecipientEmail"),email);

        helper.sendKeys(senderName,"Automation");

        helper.sendKeys(By.name("SenderEmail"),"senderemail@yopmail.com");

        helper.sendKeys(By.name("Subject"),"Result");

        helper.sendKeys(By.name("PersonalMessage"),"Test term results");
    }



}
