package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;

/**
 * Created by Ishan on 10/17/2016.
 */
public class AuthorPage extends SetupClass {

    @Test
    public void displayOfpage() {


        helper.getURL(ENV + "authors/donald-kim");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".profile__name")), "Donald Kim");

        Assert.assertTrue(helper.getElementText(By.className("profile__profile")).contains("Lorem Ipsum is simply dummy text of the printing and"));

        Assert.assertTrue(helper.isImagePresent(By.className("profile__img")));
    }

    @Test
    public void headerElements() {


        helper.getURL(ENV + "authors/donald-kim");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".profile__name")), "Donald Kim");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".profile__title")), "Donald Credentials");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".profile__location")), "Europe");

        Assert.assertTrue(helper.isImagePresent(By.className("profile__img")));

        Assert.assertEquals(helper.getElementsText(By.cssSelector(".profile__links>li>a")), Arrays.asList("donaldscrip@gmail.com","@ScripDonald","Donald Kim"));

    }
}
