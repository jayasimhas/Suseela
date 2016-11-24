package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;

/**
 * Created by Ishan on 10/17/2016.
 */
public class CompanyPage extends SetupClass {

    @Test
    public void displayOfpage() {


        helper.getURL(ENV + "deals/199220046");

        Assert.assertTrue(helper.isImagePresent(By.cssSelector(".deal-sponsor>img")));

        Assert.assertEquals(helper.getElementText(By.cssSelector(".deal-sponsor")), "Brought to you by");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".generic-content>h1")), "Hycor Biomedical licensed leukocyte tech. from Allergifonden");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".generic-content__dateline")), "Feb 1992");


        Assert.assertEquals(helper.getElementsText(By.cssSelector(".generic-content>div>strong")), Arrays.asList("Hycor Biomedical", "Allergifonden AF"));

       // Assert.assertTrue(helper.getElementText(By.className("profile__profile")).contains("Lorem Ipsum is simply dummy text of the printing and"));

        Assert.assertEquals(helper.getElementsText(By.cssSelector(".l-columns__column>h6")), Arrays.asList("DEAL INDUSTRY","DEAL STATUS","DEAL TYPE"));

        Assert.assertEquals(helper.getElementsText(By.cssSelector(".article-topics__li>ul")), Arrays.asList("Chemistry, Immunoassay","Marketing-Licensing"));

        Assert.assertEquals(helper.getElementsText(By.cssSelector(".article-topics>ul>li")), Arrays.asList("Stratagene Corp.", "Allergifonden AF","Hycor Biomedical Inc."));


    }

    @Test
    public void relatedCompaniesLink() {


        helper.getURL(ENV + "deals/199220046");

        Assert.assertEquals(helper.getElementsText(By.cssSelector(".article-topics>ul>li")), Arrays.asList("Stratagene Corp.", "Allergifonden AF", "Hycor Biomedical Inc."));

        helper.click(By.cssSelector(".article-topics>ul>li:nth-child(1)>a"));

        helper.waitForSeconds(4);

        Assert.assertEquals(driver.getCurrentUrl(), ENV + "companies/198601286");


    }

//    @Test
//    public void tootTip() {
//
//        By toolTip = By.xpath("//div[67]/div[4]");
//
//        helper.getURL(ENV + "search#?q=test");
//
//        Actions actions = new Actions(driver);
//        WebElement mainMenu = driver.findElement(By.xpath("//section[3]/div[1]/div/div[3]"));
//        actions.moveToElement(mainMenu);
//        actions.build().perform();
//
//        Assert.assertTrue(helper.isElementPresent(toolTip));
//
//    }
}
