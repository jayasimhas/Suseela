package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
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
public class Search extends SetupClass {


    @Test
    public void searchFunction(){


        String searchString1= "test";
        String searchString2= "sghb345ndfb";
        int count=0;

//        if(BROWSER.equalsIgnoreCase("Mobile")){
//            helper.getURL(ENV + "search#?q=" + searchString1);
//        }
//        else {
        helper.getURL(ENV);

        helper.waitTillElementLocated(By.name("SearchTerm"));
        driver.findElement(By.name("SearchTerm")).clear();
        driver.findElement(By.name("SearchTerm")).sendKeys(searchString1);

        driver.findElement(By.name("SearchTerm")).sendKeys(Keys.RETURN);


        helper.waitForSeconds(6);


        SearchPage searchPage = new SearchPage(driver);
        count = searchPage.getCount(searchString1);
        Assert.assertTrue(count >= 5);



        searchPage.performSearch(searchString2);
        String toVerify = driver.findElement(By.xpath("//div/div/section[1]/div")).getText();
        helper.log(toVerify);


        Assert.assertTrue(toVerify.contains("Showing 0 - 0 of 0 results for"));
        Assert.assertTrue(toVerify.contains(searchString2));

    }




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

        }

        List<String> facetText= Arrays.asList("In Vivo", "Medtech Insight", "Pink Sheet", "Rose Sheet", "Scrip");

       List<String> actual= helper.getElementsText(By.xpath("//form[1]//fieldset/div/ul/li/label/span"));

        for (int i=0;i<actual.size();i++){

            helper.log("Comparing:" +actual.get(i)+" and " +facetText.get(i));
            Assert.assertTrue(actual.get(i).contains(facetText.get(i)));

        }

    }

    @Test
    public void loggedinUser() {


        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID2") , configuration.getString("password"));

        String newSearch = "drug"+helper.randomString();
        helper.getURL(ENV + "search#?q=" + newSearch);

        helper.click(By.cssSelector(".js-save-search"));
        searchAndVerify(newSearch);


    }


    //Capcha - no longer valid
    //@Test
    public void loggedoutUser() {

        String newSearch = "drug"+helper.randomString();
        helper.getURL(ENV + "search#?q=" + newSearch);

        helper.click(By.cssSelector(".js-save-search"));

        helper.waitTillElementLocated(By.name("username"));

        helper.sendKeys(By.xpath("//form/div[1]/input[1]"),configuration.getString("emailID2") );

        helper.sendKeys(By.xpath("//form/div[1]/input[2]"), configuration.getString("password"));


        searchAndVerify(newSearch);

    }

    private void searchAndVerify(String newSearch) {
        By removeLocator =By.cssSelector(".js-lightbox-modal-trigger");

        helper.waitForSeconds(4);

        By signinSubmit = By.cssSelector(".pop-out__sign-in-submit");

        helper.click(signinSubmit);

        helper.waitForSeconds(10);

        helper.getURL(ENV + "my-account/saved-searches");

        helper.waitForSeconds(4);

        Assert.assertTrue(helper.isElementPresent(By.linkText(newSearch)));

        Assert.assertEquals(driver.findElement(By.linkText(newSearch)).getAttribute("href"), ENV + "search#?q=" + newSearch);

        for (WebElement element:driver.findElements(removeLocator)){

            element.click();

            helper.waitForSeconds(2);
            helper.click(By.cssSelector(".lightbox-modal__submit"));

        }
    }




}
