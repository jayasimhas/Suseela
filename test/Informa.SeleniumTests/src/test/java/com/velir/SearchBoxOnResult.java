package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.SearchPage;
import com.velir.utilities.Helper;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by ishan.kumar on 3/29/2016.
 */
public class SearchBoxOnResult extends SetupClass {


    @Test
    public void searchFunction(){


        String searchString1= "test";
        String searchString2= "sghb345ndfb";
        int count=0;

        if(BROWSER.equalsIgnoreCase("Mobile")){
            helper.getURL(ENV + "search#?q=" + searchString1);
        }
        else {
        helper.getURL(ENV);

        helper.waitTillElementLocated(By.name("SearchTerm"));
        driver.findElement(By.name("SearchTerm")).clear();
        driver.findElement(By.name("SearchTerm")).sendKeys(searchString1);

        driver.findElement(By.name("SearchTerm")).sendKeys(Keys.RETURN); }


        helper.waitForSeconds(6);


        SearchPage searchPage = new SearchPage(driver);
        count = searchPage.getCount(searchString1);
        Assert.assertTrue(count >= 5);



        searchPage.performSearch(searchString2);
        String toVerify = driver.findElement(By.xpath("//div/div/section[1]/div")).getText();
        helper.log(toVerify);


        Assert.assertTrue(toVerify.contains("Showing 1 - 0 of 0 results for"));
        Assert.assertTrue(toVerify.contains(searchString2));

    }


//    @Test
//    public void searchFunctionMobile(){
//
//
//        String searchString1= "test";
//        String searchString2= "sghb345ndfb";
//        int count =0;
//        helper.getURL(ENV + "search#?q=" + searchString1);
//
//
//        helper.waitForSeconds(2);
//
//        SearchPage searchPage = new SearchPage(driver);
//        count = searchPage.getCount(searchString1);
//        Assert.assertTrue(count >= 5);
//
//
//        searchPage.performSearch(searchString2);
//        String toVerify = driver.findElement(By.xpath("//div/div/section[1]/div")).getText();
//        helper.log(toVerify);
//
//
//        Assert.assertTrue(toVerify.contains("Showing 1 - 0 of 0 results for"));
//        Assert.assertTrue(toVerify.contains(searchString2));
//
//    }

    @Test
    public void resultCount(){

//       Execute a search and view the search results page
        helper.getURL(ENV + "search#?q=test");

        String actual = driver.findElement(By.cssSelector(".search-metadata.ng-scope>div")).getText();

        helper.log(actual);

//        Verify the result count displays correctly
        Assert.assertTrue(actual.contains("Showing 1 - 10 of"));

        Assert.assertTrue(actual.contains("results for \u201ctest\u201d"));


        SearchPage searchPage = new SearchPage(driver);
        searchPage.performSearch("qa");

        actual = driver.findElement(By.cssSelector(".search-metadata.ng-scope>div")).getText();


        helper.log(actual);
        //        Verify the result count displays correctly
        Assert.assertTrue(actual.contains("results for \u201cqa\u201d"));

    }


    @Test
    public void publicationFacets(){

//       Execute a search and view the search results page
        helper.getURL(ENV + "search#?q=test");
        helper.waitForSeconds(4);

        if (BROWSER.equalsIgnoreCase("Mobile")){
            helper.waitForSeconds(4);

            helper.click(By.cssSelector(".facets__mobile-button.facets__mobile-button--show"));
            //helper.click(By.xpath("//div/div/div/button[2]"));
        }

        List<String> facetText= Arrays.asList("In Vivo", "Medtech Insight", "Pink Sheet", "Rose Sheet", "Scrip");

       List<String> actual= helper.getElementsText(By.xpath("//form[1]//fieldset/div/ul/li/label/span"));

        for (int i=0;i<actual.size();i++){

            helper.log("Comparing:" +actual.get(i)+" and " +facetText.get(i));
            Assert.assertTrue(actual.get(i).contains(facetText.get(i)));

        }

    }



    @Test
    public void deviceAreaFacet(){

//       Execute a search and view the search results page
        helper.getURL(ENV + "search#?q=test");

        if (BROWSER.equalsIgnoreCase("Mobile")){
        helper.waitForSeconds(4);

            helper.click(By.cssSelector(".facets__mobile-button.facets__mobile-button--show"));
        //helper.click(By.xpath("//div/div/div/button[2]"));
        }

        List<String> facetText= Arrays.asList("Cancer", "In Vitro Diagnostics", "Cardiology", "Neuology", "Metabolic");

        List<String> actual;


        if (ENV.equalsIgnoreCase("https://scrip-pandora-4s986q61u7j8.pharmamedtechbi.com/"))
        { helper.waitForSeconds(4);
            By facetItems1= By.xpath("//form[9]/fieldset/div/ul/li/label/span");
         actual= helper.getElementsText(facetItems1);
            helper.log(actual);
        helper.click(By.xpath("//form[9]//fieldset/div[5]/ul/li/label/span"));
        helper.waitForSeconds(4);
        helper.click(By.xpath("//form[7]/fieldset/button[1]"));
            Assert.assertTrue(driver.findElements(By.xpath("//form[7]/fieldset/div/ul/li/label/span")).size()>=7);
        }
        else{
            By facetItems2= By.xpath("//form[5]/fieldset/div/ul/li/label/span");
            actual= helper.getElementsText(facetItems2);
            helper.log(actual);
            helper.click(By.xpath("//form[5]//fieldset/div[5]/ul/li/label/span"));
            helper.waitForSeconds(4);
            helper.click(By.xpath("//form[4]/fieldset/button[1]"));

            helper.waitForSeconds(2);

            Assert.assertTrue(driver.findElements(By.xpath("//form[4]/fieldset/div/ul/li/label/span")).size() >= 7);
        }

        for (int i=0;i<actual.size();i++){

            helper.log("Comparing:" +actual.get(i)+" and " +facetText.get(i));
            Assert.assertTrue(actual.get(i).contains(facetText.get(i)));

        }


        helper.waitForSeconds(4);

        //String onFacetNumber= helper.getElementText(By.xpath("//form[9]/fieldset/div[5]/ul/li/label/span/span"));
        String displayedNumber = helper.getElementText(By.xpath("//section[1]/div/strong[2]"));

        helper.log(actual.get(4));
        helper.log(displayedNumber);

        Assert.assertTrue(actual.get(4).contains(displayedNumber));


    }


    @Test
    public void industryFacet(){

//       Execute a search and view the search results page
        helper.getURL(ENV + "search#?q=test");

        if (BROWSER.equalsIgnoreCase("Mobile")){
            helper.waitForSeconds(4);

            helper.click(By.cssSelector(".facets__mobile-button.facets__mobile-button--show"));
            //helper.click(By.xpath("//div/div/div/button[2]"));
        }

        List<String> facetText= Arrays.asList("Medical Device", "BioPharmaceutical", "Consumer");

        List<String> actual;


        if (ENV.equalsIgnoreCase("https://scrip-pandora-4s986q61u7j8.pharmamedtechbi.com/"))
        { By facetItems1= By.xpath("//form[10]/fieldset/div/ul/li/label/span");
            actual= helper.getElementsText(facetItems1);
            helper.log(actual);
            helper.click(By.xpath("//form[10]//fieldset/div[3]/ul/li/label/span"));
            //helper.waitForSeconds(4);
            //helper.click(By.xpath("//form[7]/fieldset/button[1]"));
            //Assert.assertTrue(driver.findElements(By.xpath("//form[7]/fieldset/div/ul/li/label/span")).size()>=7);
        }
        else{
            By facetItems2= By.xpath("//form[2]/fieldset/div/ul/li/label/span");
            actual= helper.getElementsText(facetItems2);
            helper.log(actual);
            helper.click(By.xpath("//form[2]//fieldset/div[3]/ul/li/label/span"));
            //helper.waitForSeconds(4);
            //helper.click(By.xpath("//form[4]/fieldset/button[1]"));
            //Assert.assertTrue(driver.findElements(By.xpath("//form[4]/fieldset/div/ul/li/label/span")).size() >= 7);
        }
        helper.waitForSeconds(2);

        for (int i=0;i<actual.size();i++){

            helper.log("Comparing:" +actual.get(i)+" and " +facetText.get(i));
            Assert.assertTrue(actual.get(i).contains(facetText.get(i)));

        }


        helper.waitForSeconds(4);

        //String onFacetNumber= helper.getElementText(By.xpath("//form[9]/fieldset/div[5]/ul/li/label/span/span"));
        String displayedNumber = helper.getElementText(By.xpath("//section[1]/div/strong[2]"));

        helper.log(actual.get(2));
        helper.log(displayedNumber);

        Assert.assertTrue(actual.get(2).contains(displayedNumber));


    }




    public void headlines(){

        helper.getURL(ENV + "search#?q=test");

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".result__description.ng-binding")));

        helper.click(By.cssSelector(".onoffswitch-label"));

        Assert.assertFalse(driver.findElement(By.cssSelector(".result__description.ng-binding")).isDisplayed());


    }



    public void searchTips(){

        helper.getURL(ENV + "search#?q=test");

        By searchTipLocator = By.cssSelector(".search-bar__tips-button.js-toggle-search-tips>span");

        Assert.assertTrue(helper.isElementPresent(searchTipLocator));

        Assert.assertEquals(driver.findElement(searchTipLocator).getText(), "Search Tips");

    }
}
