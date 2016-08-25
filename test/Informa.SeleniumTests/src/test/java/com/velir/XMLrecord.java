package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 7/29/2016.
 */
public class XMLrecord extends SetupClass {

    @Test
    public void dealXMLRecord() {


        helper.getURL(ENV + "/util/dcd/viewdcdxml.aspx");

        helper.waitForSeconds(2);

        driver.findElement(By.id("txtDealNumber")).sendKeys("199110001");

        helper.waitForSeconds(2);

        helper.click(By.id("btnGo"));

        helper.waitForSeconds(2);

        String actualPage = driver.getPageSource();

        Assert.assertTrue(actualPage.contains("<Characteristic>Full Acquisition</Characteristic>"));

        Assert.assertTrue(actualPage.contains("<DealDetail>The proposed acquisition price includes $1.75 million in cash"));


    }

    @Test
    public void companyXMLRecord() {


        helper.getURL(ENV + "/util/dcd/viewdcdxml.aspx");

        helper.waitForSeconds(2);

        driver.findElement(By.id("txtCompanyNumber")).sendKeys("199900576");

        helper.waitForSeconds(2);

        helper.click(By.id("btnGo"));

        helper.waitForSeconds(2);

        String actualPage = driver.getPageSource();

        Assert.assertTrue(actualPage.contains("<CompanyPath id=\"10958\">A-Dec Inc.</CompanyPath>"));

        Assert.assertTrue(actualPage.contains("<Street>2601 Crestview Drive</Street>"));


    }
}
