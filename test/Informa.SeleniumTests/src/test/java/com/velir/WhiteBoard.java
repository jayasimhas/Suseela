package com.velir;

import com.google.common.collect.Ordering;
import com.velir.baseclass.SetupClass;
import com.velir.pageobject.GeneralPage;
import com.velir.pageobject.WhiteBoardPage;
import com.velir.utilities.Helper;
import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.Select;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.time.format.DateTimeFormatter;
import java.util.*;

/**
 * Created by ishan.kumar on 3/25/2016.
 */
public class WhiteBoard extends SetupClass {


    private void navigateAndLogin() {
        driver.get(ENV_BE + "/login?returnUrl=/vwb");
        WhiteBoardPage page = new WhiteBoardPage(driver);
        page.boardLogin(configuration.getString("Auth.username"), configuration.getString("Auth.password"));
        driver.get("https://calypso-8yra6ecdjk6n.pharmamedtechbi.com/vwb?sd=080120161200AM&ed=082220161200AM&max=250&run=1&pubCodes=INV,MTI,PINK,ROSE,SCRIP&sc_mode=normal");
    }


    @Test
    public void downloadArticle(){
        By fileLocator = By.linkText("SC071218");

        navigateAndLogin();

        helper.log(driver.findElement(fileLocator).getAttribute("href"));
        Assert.assertTrue(driver.findElement(fileLocator).getAttribute("href").contains(".docx?sc_mode=preview"));

    }



    @Test
    public void previewArticle(){
        By titleLocator = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[3]/a");

        navigateAndLogin();

        //click title sort
        //helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(3)>td:nth-child(3)>a"));

        helper.waitForSeconds(2);
        driver.findElement(titleLocator).click();
        helper.waitForSeconds(4);

        ArrayList<String> tabs2 = new ArrayList<String>(driver.getWindowHandles());
        driver.switchTo().window(tabs2.get(1));
        Boolean isPresent= driver.getCurrentUrl().contains("https://calypso-8yra6ecdjk6n.pharmamedtechbi.com/?sc_itemid={");
        driver.close();
        driver.switchTo().window(tabs2.get(0));

        Assert.assertTrue(isPresent);

    }


    @Test
    public void addColumnAndSortingTitle(){

        helper.log("Description - Verify Title sorting in asc. and dec. order");

        navigateAndLogin();

        helper.waitForSeconds(2);

        Select dropdown1 =new Select(driver.findElement(By.cssSelector("#ddColumns")));
        dropdown1.selectByValue("acdt");

        helper.waitForSeconds(2);
        Assert.assertEquals(helper.getElementText(By.xpath(".//*[@id='tblResults']/tbody/tr[1]/td[4]/span")), "Created Date");


        //click title sort
        helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(2)>td:nth-child(3)>a"));
        // helper.waitForSeconds(4);

        helper.waitForSeconds(2);
        List<WebElement> actual2=driver.findElements(By.cssSelector("#tblResults>tbody>tr:nth-child(n+3)>td:nth-child(3)"));
        LinkedList<Character> actualTexts = new LinkedList<Character>();
        for (WebElement we: actual2){
            actualTexts.add(we.getText().toUpperCase().charAt(0));
        }

        List<Character> actual = new ArrayList<Character>(actualTexts);
        helper.log(actual);
        Assert.assertTrue(Ordering.natural().isOrdered(actual));



    }

    @Test
    public void showInProgress(){
        By fileLocator = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[1]/a");

       navigateAndLogin();

        Select dropdown1 =new Select(driver.findElement(By.cssSelector("#ddColumns")));
        dropdown1.selectByValue("wsc");

        helper.click(By.id("chkShowInProgressArticles"));

        By stateValue = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[3]");

        Assert.assertFalse(helper.getElementText(stateValue).contains("Read for production"));

        Assert.assertFalse(helper.getElementText(stateValue).contains("Edit After publish"));

    }








}
