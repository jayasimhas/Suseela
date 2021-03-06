package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by ishan.kumar on 7/21/2016.
 */
public class RSSFeed extends SetupClass {

    @Test
    public void rssSearchPage() {

//       Execute a search and view the search results page
        helper.getURL(ENV + "search#?q=InformaTestQA");


        helper.waitForSeconds(2);

      String expectedTitle = helper.getElementText(By.xpath("//section[3]/div[1]/div/a"));
        String expectedHref = driver.findElement(By.xpath("//section[3]/div[1]/div/a")).getAttribute("href");

        helper.log(expectedTitle +"space"+expectedHref);

        By rssLocator = By.cssSelector(".search__header-buttons--rss");
        helper.click(rssLocator);

        String urlSource= driver.findElement(rssLocator).getAttribute("href");


        helper.getURL(urlSource);

        helper.waitForSeconds(4);
       // List<String> expectedTags = Arrays.asList("<title>", "<link>","<description>", "<language>", "<copyright>", "<webMaster>", "<image>","</image>","<pubDate>","<guid>");
       Assert.assertTrue(driver.getPageSource().contains(expectedTitle));

        Assert.assertTrue(driver.getPageSource().contains(expectedHref));


    }

    @Test
    public void rssFeedPublication() {

//       Execute a search and view the search results page
        helper.getURL(ENV + "rss/Publications-Feed.aspx");


        helper.waitForSeconds(6);


        List<String> expectedTags = Arrays.asList("<title>", "<link>","<description>", "<language", "<copyright>", "<webMaster","<pubDate","<guid");

        String actualPageSource= driver.getPageSource();
        for (String str:expectedTags) {
            helper.log(str);
            helper.waitForSeconds(2);
            Assert.assertTrue(actualPageSource.contains(str));
            helper.waitForSeconds(2);
        }



    }

}
