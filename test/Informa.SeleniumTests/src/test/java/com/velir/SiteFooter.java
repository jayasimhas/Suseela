package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.openqa.selenium.Point;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by ishan.kumar on 3/10/2016.
 */
public class SiteFooter extends SetupClass {




    @Test
    public void footerLogo(){

        helper.log("Description: Verify footer image is present and directing to correct link");
        helper.getURL(ENV);
        By footerLogo = By.cssSelector(".footer__logo");

        String actualSrc= driver.findElement(footerLogo).getAttribute("src");
       // System.out.println(actualSrc);

        helper.log("Image source is:" + actualSrc);
        String expectedSubURL= expectedData.getString("footerLogo.partialURL");
        Assert.assertEquals(actualSrc, ENV + expectedSubURL);

        Assert.assertTrue(helper.isImagePresent(footerLogo));
    }

    @Test
    public void copyrightText(){

        helper.log("Description: Verifying copyright Text");
        By text =By.cssSelector(".footer__section.footer__section--corporate>li");
        helper.getURL(ENV);

        helper.log("Actual Text is:" + driver.findElement(text).getText());
        Assert.assertEquals(driver.findElement(text).getText(), expectedData.getString("copyrightText.data"));

    }


    //subscribe not working
    @Test
    public void localFooterLinks(){

        helper.log("Description: Verifying footer text and link redirect");
        helper.getURL(ENV);
        By allLinks = By.cssSelector(".footer__section>ul>li>a");

        List<String> expectedFooterText = Arrays.asList("Subscribe", "Home", "Test Article", "Home", "Test Article", "Test Article", "Home");
        List<String> expectedFooterLinks= Arrays.asList(ENV, ENV, ENV+"test-article", ENV, ENV+"test-article", ENV+"test-article",ENV);

       // System.out.println(helper.getElementsLinks(allSocials));
        helper.log(helper.getElementsLinks(allLinks));

       // System.out.println(helper.getElementsLinks(allLinks));

        Assert.assertEquals(helper.getElementsText(allLinks), expectedFooterText);
        Assert.assertEquals(helper.getElementsLinks(allLinks), expectedFooterLinks);


    }

    @Test
    public void socialLinks() {

        helper.getURL(ENV);

        helper.log("Description: Verifying social links and if they open in new tabs ");
        List<String> expectedSocialLinks= Arrays.asList("http://www.linkedin.com/", "http://www.twitter.com/", "http://www.facebook.com/");
        List<String>  logoPaths =Arrays.asList("/dist/img/svg-sprite.svg#linkedin-footer", "/dist/img/svg-sprite.svg#twitter-footer", "/dist/img/svg-sprite.svg#facebook-footer");
        List<String>  newTab = Arrays.asList("_blank","_blank","_blank");
        By allSocials=By.cssSelector(".footer__section.footer__section--social>li>a");
        By socialLogo=By.cssSelector(".footer__section.footer__section--social>li>a>svg>use");
        Assert.assertEquals(helper.getElementsLinks(allSocials), expectedSocialLinks);
        Assert.assertEquals(helper.getElementsAttribute(allSocials, "target"),newTab);

        //helper.log(helper.getElementsAttribute(socialLogo,"xlink:href"));

        //Assert.assertEquals(helper.getElementsAttribute(socialLogo,"xlink:href"),logoPaths);
    }

    @Test
    public void menuOne(){

        helper.log("Description: Menu One options are arranged linear");
        helper.getURL(ENV);
        By menuOneLocator = By.cssSelector(".footer__section.footer__section--single>ul>li>a");
        By menuOneName =By.cssSelector(".footer__wrapper>div:nth-child(5)>span");
        String expectedName ="MENU ONE HEADER";

       // List<String> expectedFooterText = Arrays.asList("Home", "Test Article");
       // List<String> expectedFooterLinks= Arrays.asList(ENV, ENV + "test-article");

        // System.out.println(helper.getElementsLinks(allSocials));


        // System.out.println(helper.getElementsLinks(allLinks));
        List<WebElement> elements=driver.findElements(menuOneLocator);
        LinkedList<Point> actualPoints = new LinkedList<>();
        for (WebElement we: elements){

            actualPoints.add(we.getLocation());
        }

        helper.log(actualPoints);
        Assert.assertEquals(actualPoints.get(0).getX(), actualPoints.get(1).getX());
        helper.log("Found X points are equal");
        Assert.assertNotEquals(actualPoints.get(0).getY(), actualPoints.get(1).getY());
        helper.log("Found Y points are not equal");
        Assert.assertEquals(driver.findElement(menuOneName).getText(),expectedName);
        //Assert.assertEquals(helper.getElementsText(allLinks), expectedFooterText);
        //Assert.assertEquals(helper.getElementsLinks(allLinks), expectedFooterLinks);


    }


    @Test
    public void menuTwo(){

        helper.log("Description: Menu Two options are stacked");
        helper.getURL(ENV);
        By menuOneLocator = By.cssSelector(".footer__section.footer__section--triple>ul>li>a");
        By menuTwoName =By.cssSelector(".footer__wrapper>div:nth-child(6)>span");
        String expectedName ="MENU TWO HEADER";

        List<WebElement> elements=driver.findElements(menuOneLocator);
        LinkedList<Point> actualPoints = new LinkedList<>();
        for (WebElement we: elements){

            actualPoints.add(we.getLocation());
        }

        helper.log("Actual points:" + actualPoints);
        Assert.assertEquals(actualPoints.get(0).getY(), actualPoints.get(1).getY());
        helper.log("Found Y points are equal");
        Assert.assertNotEquals(actualPoints.get(0).getX(), actualPoints.get(1).getX());
        helper.log("Found X points are not equal");
        Assert.assertEquals(driver.findElement(menuTwoName).getText(),expectedName);
        //Assert.assertEquals(helper.getElementsText(allLinks), expectedFooterText);
        //Assert.assertEquals(helper.getElementsLinks(allLinks), expectedFooterLinks);


    }

    @Test
    public void followScript(){

        helper.log("Description - Verify Follow Script element is present and text is present");
        helper.getURL(ENV);
        By followText =By.cssSelector(".footer__section--social>li:nth-child(1)");
        Assert.assertEquals(driver.findElement(followText).getText(),expectedData.getString("followScript.expectedName"));

    }


    //not functional
    //@Test
    public void subscribeLoggedIn(){

        helper.getURL(ENV);


    }

    //not functional
    //@Test
    public void subscribeLoggedOut(){

        helper.getURL(ENV);


    }


    @Test
    public void mobileView(){

        helper.log("Description - All menu options are stacked");

        helper.getURL(ENV);
        By allLinks = By.cssSelector(".footer__section>ul>li>a");

        List<WebElement> elements=driver.findElements(allLinks);
        final LinkedList<Integer> actualPoints = new LinkedList<>();
        for (WebElement we: elements){

            actualPoints.add(we.getLocation().getX());
        }

      //  boolean match = actualPoints.stream().allMatch(s -> s.equals(actualPoints.get(0)));

        helper.log("Actual Points:"+actualPoints);
      //  Assert.assertTrue(match);

        List<Integer> expectedPoints = Arrays.asList(142,159,146);

        Assert.assertTrue(actualPoints.containsAll(expectedPoints));





    }
}
