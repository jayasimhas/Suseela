package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.List;

/**
 * Created by ishan.kumar on 8/18/2016.
 */
public class SavedDocuments extends SetupClass {

    @Test
    public void itemsOnPage() {

        By title = By.cssSelector(".sortable-table__col.js-sortable-table-sorter.l-no-wrap");


        By article=By.xpath("//table/tbody/tr[1]/td");

        List<String> expectedArticle = Arrays.asList("In Vivo", "Genzyme: Why Diversification is Starting to Look Smart", "1 Nov 2016", "REMOVE");


        List<String> expected = Arrays.asList("Publication", "Title", "Date");
        //List<String> expectedMobile = Arrays.asList("SORT BY:", "Title", "Date");

        navigateLoginBookmarkPage();


            Assert.assertEquals(helper.getElementsText(title), expected);
 //       }

        helper.log(helper.getElementsText(article));
        Assert.assertTrue(helper.getElementsText(article).containsAll(expectedArticle));



    }


    @Test
    public void removeButton() {

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        By cancelLocator = By.cssSelector(".dismiss-button.js-close-lightbox-modal>svg");


        By removeLocator =By.cssSelector(".js-lightbox-modal-trigger");

        homePage.loginProcess(configuration.getString("emailID2") , configuration.getString("password"));


        helper.getURL(ENV + "SC071032/GlycoVaxyn-AG");

        //bookmarking
        helper.click(By.cssSelector(".action-flags.article-prologue__share-bar>ul>li:nth-child(4)"));

        //helper.click(By.cssSelector(".pop-out__sign-in-submit"));

        helper.getURL(ENV + "my-account/saved-articles");

        helper.click(removeLocator);

        helper.waitTillElementLocated(cancelLocator);

        helper.click(cancelLocator);

        Assert.assertFalse(driver.findElement(cancelLocator).isDisplayed());

        for (WebElement element:driver.findElements(removeLocator)){

            helper.waitForSeconds(1);
            element.click();

            helper.waitForSeconds(2);
            helper.click(By.cssSelector(".lightbox-modal__submit"));
            helper.log("Removing bookmarked article");

        }



        Assert.assertFalse(helper.isElementPresent(By.linkText("GlycoVaxyn AG")));
        helper.log("Bookmarked article is not present anymore");

    }


    private void navigateLoginBookmarkPage() {
        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") , configuration.getString("password"));

        helper.getURL(ENV + "my-account/saved-articles");


    }
}
