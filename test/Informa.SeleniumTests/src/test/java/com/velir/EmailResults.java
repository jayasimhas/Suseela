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

    //By sendLocator = By.xpath("//ul/li[3]/div[2]/div[2]/div[3]/form/div/div[2]/button");

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


    //Don't access yopmail on Mobile
    @Test
    public void functionOfcomponent(){

        sendLocator = By.xpath("//div[2]/div[3]/form/div/div[2]/button");


        helper.getURL(ENV + "/search#?q=test");

        fillForm("ishankumarinforma");


        helper.click(sendLocator);


        helper.waitForSeconds(4);

        String invalidEmail = helper.getElementText(By.cssSelector(".js-form-error-EmailRequirements"));



        if (!(BROWSER.equalsIgnoreCase("Mobile"))) {

            Assert.assertEquals(invalidEmail, "Please enter a valid e-mail address");


            helper.sendKeys(By.name("RecipientEmail"), "ishankumarinforma@yopmail.com");

            helper.click(sendLocator);

            helper.waitForSeconds(2);


            String allSet = driver.findElement(By.cssSelector("div.alert-success.js-email-search-success > p")).getText();


            Assert.assertEquals(allSet, "All set! This search has been sent to ishankumarinforma@yopmail.com.");


            helper.getURL("http://ishankumarinforma.yopmail.com/en/");

            helper.waitForSeconds(4);

            driver.switchTo().frame("ifmail");
            helper.waitForSeconds(2);

            Date date = new Date();


            SimpleDateFormat ft1 =
                    new SimpleDateFormat("d");

            SimpleDateFormat ft2 =
                    new SimpleDateFormat("E");

            Assert.assertEquals(helper.getElementText(By.xpath(".//*[@id='mailhaut']/div[1]")), "Result");

            //verify date is correct
            Assert.assertTrue(helper.getElementText(By.cssSelector(".preheaderContent")).contains(ft1.format(date)));

            //verify day is correct
            Assert.assertTrue(helper.getElementText(By.cssSelector(".preheaderContent")).contains(ft2.format(date)));



        }



    }
}
