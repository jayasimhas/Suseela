package com.velir.utilities;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Action;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Reporter;


import java.lang.reflect.Field;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.Random;
import java.util.concurrent.TimeUnit;

/**
 * Created by ishan.kumar on 7/23/2015.
 */
public class Helper {

    protected WebDriver driver;

    public Helper(WebDriver driver){
        this.driver =driver;
    }

    //implicit wait
    public void waitForSeconds(final int seconds) {
        try {
            Thread.sleep(seconds * 1000);
        } catch (InterruptedException ignore) { }
    }


    //wait till element is located (explicit wait)
    public void waitTillElementLocated(By elementLocator){

        WebDriverWait wait=new WebDriverWait(driver,25);
        wait.until(ExpectedConditions.presenceOfElementLocated(elementLocator));

    }

    //check if Image is present
    public boolean isImagePresent(By imageByObject){

        WebElement logoImage = driver.findElement(imageByObject);

        Boolean imageLoaded1 = (Boolean) ((JavascriptExecutor)driver).executeScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", logoImage);
        if (!imageLoaded1) return false;
        else return true;

    }

    //get Elements Texts
    public List<String> getElementsText(By byObject){
        List<WebElement> elements=driver.findElements(byObject);
        LinkedList<String> actualTexts = new LinkedList<String>();
        for (WebElement we: elements){
            actualTexts.add(we.getText());
        }
        return  actualTexts;
    }

    //get Single Element Text
    public String getElementText(By byObject){

        waitTillElementLocated(byObject);
        return  driver.findElement(byObject).getText();
    }


    //get Driver URL
    public void getURL(String url){
        driver.get(url);
        waitForSeconds(4);
    }

    //get Elements Links
    public List<String> getElementsLinks(By byObject){
        List<WebElement> elements=driver.findElements(byObject);
        LinkedList<String> actualLinks = new LinkedList<String>();
        for (WebElement we: elements){

            actualLinks.add(we.getAttribute("href"));
        }
        return  actualLinks;
    }


    //get Elements Attributes
    public List<String> getElementsAttribute(By byObject, String attribute){
        List<WebElement> elements=driver.findElements(byObject);
        LinkedList<String> actualLinks = new LinkedList<String>();
        for (WebElement we: elements){

            actualLinks.add(we.getAttribute(attribute));
        }
        return  actualLinks;
    }

    // get Elements CSS Value
    public List<String> getElementsCSSValue(By byObject,String CSSvalue){
        List<WebElement> elements=driver.findElements(byObject);
        LinkedList<String> actualLinks = new LinkedList<String>();
        for (WebElement we: elements){

            actualLinks.add(we.getCssValue(CSSvalue));
        }
        return  actualLinks;
    }


    //Check if Element is Present
    public boolean isElementPresent(By byObject){
        waitForSeconds(4);
        return driver.findElements(byObject).size()> 0;
    }

    //Converts to array of strings
    public String[] convertToArray(By locator){
        String summaryText= driver.findElement(locator).getText();
        return summaryText.split("\\ ", -1);
    }

    //Check if Text Present
    public static boolean isTextPresent(String str){

        if(str != null && !str.isEmpty()){
            return true;
        }
        else {
            return false;
        }
    }


    public boolean allElementsContain(List<String> actualList,String expectedString){
        for (String actualString: actualList){
            if (!actualString.contains(expectedString)){
                return false;
            }
        }
        return  true;
    }


    //Drag and Drop Operation
    public void dragAndDrop(By drag, By drop, WebDriver driver){
        driver.manage().window().maximize();

        driver.manage().timeouts().implicitlyWait(10000, TimeUnit.MILLISECONDS);

        WebElement From = driver.findElement(drag);

        WebElement To = driver.findElement(drop);

        Actions builder = new Actions(driver);

        Action dragAndDrop = builder.clickAndHold(From)

                .moveToElement(To)

                .release(To)

                .build();

        dragAndDrop.perform();
        waitForSeconds(4);
    }

    //click Operation
    public void click(By byObject){
        waitTillElementLocated(byObject);
        driver.findElement(byObject).click();
        waitForSeconds(2);
    }

    //click on the co-ordinate of an element
    public void clickLocation(By locator, String axis){


        waitTillElementLocated(locator);

        WebElement clickReg = driver.findElement(locator);


        if (axis.equalsIgnoreCase("x")){
            ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().x + ")");}
        else if (axis.equalsIgnoreCase("y"))
            ((JavascriptExecutor)driver).executeScript("window.scrollTo(0," + clickReg.getLocation().y + ")");


        waitForSeconds(4);
        clickReg.click();
        waitForSeconds(2);
    }

    //SEND KEYS
    public void sendKeys(By byObject,String typeKeys){
        waitTillElementLocated(byObject);
        WebElement element = driver.findElement(byObject);
        element.clear();
        element.sendKeys(typeKeys);
        waitForSeconds(1);
    }



    public String randomString(){
    char[] chars = "abcdefghijklmnopqrstuvwxyz".toCharArray();
    StringBuilder sb = new StringBuilder();
    Random random = new Random();
    for (int i = 0; i < 6; i++) {
        char c = chars[random.nextInt(chars.length)];
        sb.append(c);
    }
    String output = sb.toString();
    System.out.println(output);
    return output;
    }

    //Logs Console and TestNG Report
    public void log(Object description){

        Date d = new Date();

        Reporter.log("<b>" + d.toString() + " :: </b>" + description.toString() + "<br>", true);

    }

    }

