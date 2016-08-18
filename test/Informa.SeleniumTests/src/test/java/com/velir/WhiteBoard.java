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
        page.boardLogin(configuration.getString("Auth.username"),configuration.getString("Auth.password"));
    }


    @Test
    public void downloadArticle(){
        By fileLocator = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[1]/a");

        navigateAndLogin();

        helper.click(fileLocator);
        Assert.assertTrue(driver.findElement(fileLocator).getAttribute("href").contains(".docx?sc_mode=preview"));

    }



    @Test
    public void previewArticle(){
        By titleLocator = By.xpath(".//*[@id='tblResults']/tbody/tr[3]/td[2]/a");

        navigateAndLogin();
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
        Assert.assertEquals(helper.getElementText(By.xpath(".//*[@id='tblResults']/tbody/tr[1]/td[3]/span")), "Created Date");


        helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(2)>td:nth-child(2)>a"));
        // helper.waitForSeconds(4);

        helper.waitForSeconds(2);
        List<WebElement> actual2=driver.findElements(By.cssSelector("#tblResults>tbody>tr:nth-child(n+3)>td:nth-child(2)"));
        LinkedList<String> actualTexts = new LinkedList<String>();
        for (WebElement we: actual2){
            actualTexts.add(we.getText().toLowerCase());
        }

        List<String> actual = new ArrayList<String>(actualTexts);
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


//    @Test
//    public void copyURL()  {
//
//
//        driver.get(ENV + "/sitecore/login?returnUrl=/vwb");
//        WhiteBoardPage page = new WhiteBoardPage(driver);
//
//        page.boardLogin();
//
//        new Select(driver.findElement(By.id("ddColumns"))).selectByVisibleText("Author(s)");
//
//        helper.waitForSeconds(4);
//        new Select(driver.findElement(By.id("ddColumns"))).selectByVisibleText("Created Date");
//        helper.waitForSeconds(4);
//
//        String currentURL = driver.getCurrentUrl();
//
//
//        driver.get("https://www.google.com/");
//        helper.waitForSeconds(2);
//        driver.get(currentURL);
//        helper.waitForSeconds(4);
//
//        Assert.assertEquals(driver.findElement(By.xpath("//table[@id='tblResults']/tbody/tr/td[3]/span")).getText(), "Author(s)");
//        Assert.assertEquals(driver.findElement(By.xpath("//table[@id='tblResults']/tbody/tr/td[4]/span")).getText(), "Created Date");
//       // driver.findElement(By.id("rbDateRange")).click();
//    }



//    @Test
//    public void sortingTitle(){
//
//        helper.log("Description - Verify Title sorting in asc. and dec. order");
//
//        driver.get(ENV + "/sitecore/login?returnUrl=/vwb");
//        WhiteBoardPage page = new WhiteBoardPage(driver);
//
//        page.boardLogin();
//
//       // List<String> actual2 = helper.getElementsText();
//        helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(2)>td:nth-child(2)>a"));
//       // helper.waitForSeconds(4);
//
//        List<WebElement> actual2=driver.findElements(By.cssSelector("#tblResults>tbody>tr:nth-child(n+3)>td:nth-child(2)"));
//        LinkedList<String> actualTexts = new LinkedList<String>();
//        for (WebElement we: actual2){
//            actualTexts.add(we.getText().toLowerCase());
//        }
//
//        List<String> actual = new ArrayList<String>(actualTexts);
//        helper.log(actual);
//        Assert.assertTrue(Ordering.natural().isOrdered(actual));
//
//
//
//    }

//
//    @Test
//    public void columnMoveButton(){
//
//        helper.log("Description - Verify Article Number and Title sorting in asc. and dec. order");
//
//        driver.get(ENV + "/sitecore/login?returnUrl=/vwb");
//        WhiteBoardPage page = new WhiteBoardPage(driver);
//
//        page.boardLogin();
//
//        // List<String> actual2 = helper.getElementsText();
//
//        helper.getURL(ENV + "vwb?cord=acdt,ath,sapdt&csort=t&run=1&sc_mode=normal");
//        helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(2)>td:nth-child(4)>a:nth-child(2)"));
//
//        Assert.assertEquals(driver.findElement(By.cssSelector("#tblResults>tbody>tr:nth-child(1)>td:nth-child(5)")).getText(), "Author(s)");
//
//        Assert.assertEquals(driver.findElement(By.cssSelector("#tblResults>tbody>tr:nth-child(1)>td:nth-child(4)")).getText(), "Planned Publish Date");
//
//        helper.click(By.cssSelector("#tblResults>tbody>tr:nth-child(2)>td:nth-child(5)>a:nth-child(1)"));
//
//        Assert.assertEquals(driver.findElement(By.cssSelector("#tblResults>tbody>tr:nth-child(1)>td:nth-child(5)")).getText(), "Planned Publish Date");
//
//        Assert.assertEquals(driver.findElement(By.cssSelector("#tblResults>tbody>tr:nth-child(1)>td:nth-child(4)")).getText(), "Author(s)");
//
//
//
//    }






}
