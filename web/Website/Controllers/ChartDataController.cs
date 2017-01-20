﻿using Informa.Model.DCD;
using Informa.Models.DCD;
using PluginModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace Informa.Web.Controllers
{
   [Route]
    public class GetChartDataController : ApiController
    {
        [HttpGet]
        public string GetChartData()
        {
            string chartData = "2017-01-18,952 2017-01-17,922 2017-01-16,925 2017-01-13,910 2017-01-12,892 2017-01-11,894 2017-01-10,926 2017-01-09,949 2017-01-06,963 2017-01-05,983 2017-01-04,969 2017-01-03,953 2016-12-23,961 2016-12-22,928 2016-12-21,926 2016-12-20,914 2016-12-19,927 2016-12-16,946 2016-12-15,966 2016-12-14,1003 2016-12-13,1052 2016-12-12,1069 2016-12-09,1090 2016-12-08,1122 2016-12-07,1162 2016-12-06,1186 2016-12-05,1196 2016-12-02,1198 2016-12-01,1196 2016-11-30,1204 2016-11-29,1202 2016-11-28,1184 2016-11-25,1181 2016-11-24,1201 2016-11-23,1224 2016-11-22,1232 2016-11-21,1240 2016-11-18,1257 2016-11-17,1231 2016-11-16,1145 2016-11-15,1084 2016-11-14,1065 2016-11-11,1045 2016-11-10,974 2016-11-09,954 2016-11-08,911 2016-11-07,870 2016-11-04,855 2016-11-03,849 2016-11-02,834 2016-11-01,838 2016-10-31,857 2016-10-28,834 2016-10-27,798 2016-10-26,802 2016-10-25,813 2016-10-24,831 2016-10-21,842 2016-10-20,849 2016-10-19,872 2016-10-18,890 2016-10-17,894 2016-10-14,892 2016-10-13,885 2016-10-12,906 2016-10-11,922 2016-10-10,922 2016-10-07,921 2016-10-06,915 2016-10-05,869 2016-10-04,860 2016-10-03,864 2016-09-30,875 2016-09-29,888 2016-09-28,912 2016-09-27,930 2016-09-26,934 2016-09-23,941 2016-09-22,937 2016-09-21,903 2016-09-20,865 2016-09-19,836 2016-09-16,800 2016-09-15,764 2016-09-14,756 2016-09-13,796 2016-09-12,804 2016-09-09,804 2016-09-08,792 2016-09-07,773 2016-09-06,745 2016-09-05,724 2016-09-02,720 2016-09-01,712 2016-08-31,711 2016-08-30,715 2016-08-26,720 2016-08-25,718 2016-08-24,706 2016-08-23,692 2016-08-22,687 2016-08-19,683 2016-08-18,682 2016-08-17,685 2016-08-16,687 2016-08-15,681 2016-08-12,671 2016-08-11,653 2016-08-10,638 2016-08-09,631 2016-08-08,636 2016-08-05,636 2016-08-04,636 2016-08-03,641 2016-08-02,645 2016-08-01,650 2016-07-29,656 2016-07-28,665 2016-07-27,679 2016-07-26,696 2016-07-25,709 2016-07-22,718 2016-07-21,726 2016-07-20,736 2016-07-19,746 2016-07-18,748 2016-07-15,745 2016-07-14,738 2016-07-13,726 2016-07-12,711 2016-07-11,704 2016-07-08,703 2016-07-07,699 2016-07-06,694 2016-07-05,692 2016-07-04,688 2016-07-01,677 2016-06-30,660 2016-06-29,640 2016-06-28,627 2016-06-27,616 2016-06-24,609 2016-06-23,596 2016-06-22,585 2016-06-21,580 2016-06-20,582 2016-06-17,587 2016-06-16,598 2016-06-15,604 2016-06-14,608 2016-06-13,609 2016-06-10,610 2016-06-09,611 2016-06-08,610 2016-06-07,606 2016-06-06,607 2016-06-03,610 2016-06-02,606 2016-06-01,612 2016-05-31,612 2016-05-27,606 2016-05-26,601 2016-05-25,605 2016-05-24,618 2016-05-23,624 2016-05-20,625 2016-05-19,634 2016-05-18,642 2016-05-17,643 2016-05-16,613 2016-05-13,600 2016-05-12,579 2016-05-11,579 2016-05-10,594 2016-05-09,616 2016-05-06,631 2016-05-05,642 2016-05-04,652 2016-05-03,682 2016-04-29,703 2016-04-28,710 2016-04-27,715 2016-04-26,704 2016-04-25,690 2016-04-22,688 2016-04-21,670 2016-04-20,669 2016-04-19,671 2016-04-18,659 2016-04-15,635 2016-04-14,597 2016-04-13,567 2016-04-12,560 2016-04-11,555 2016-04-08,539 2016-04-07,517 2016-04-06,500 2016-04-05,487 2016-04-04,471 2016-04-01,450 2016-03-31,429 2016-03-30,414 2016-03-29,409 2016-03-24,406 2016-03-23,401 2016-03-22,398 2016-03-21,398 2016-03-18,395 2016-03-17,392 2016-03-16,393 2016-03-15,396 2016-03-14,393 2016-03-11,388 2016-03-10,384 2016-03-09,376 2016-03-08,366 2016-03-07,354 2016-03-04,349 2016-03-03,342 2016-03-02,335 2016-03-01,332 2016-02-29,329 2016-02-26,327 2016-02-25,325 2016-02-24,322 2016-02-23,318 2016-02-22,316 2016-02-19,315 2016-02-18,313 2016-02-17,307 2016-02-16,301 2016-02-15,295 2016-02-12,291 2016-02-11,290 2016-02-10,290 2016-02-09,291 2016-02-08,293 2016-02-05,297 2016-02-04,298 2016-02-03,303 2016-02-02,310 2016-02-01,314 2016-01-29,317 2016-01-28,325 2016-01-27,337 2016-01-26,345 2016-01-25,354 2016-01-22,354 2016-01-21,355 2016-01-20,358 2016-01-19,363 2016-01-18,369 2016-01-15,373 2016-01-14,383 2016-01-13,394 2016-01-12,402 2016-01-11,415 2016-01-08,429 2016-01-07,445 2016-01-06,467 2016-01-05,468 2016-01-04,473 2015-12-24,478 2015-12-23,475 2015-12-22,474 2015-12-21,478 2015-12-18,477 2015-12-17,471 2015-12-16,471 2015-12-15,484 2015-12-14,508 2015-12-11,522 2015-12-10,534 2015-12-09,546 2015-12-08,551 2015-12-07,551 2015-12-04,563 2015-12-03,574 2015-12-02,590 2015-12-01,598 2015-11-30,584 2015-11-27,581 2015-11-26,562 2015-11-25,546 2015-11-24,528 2015-11-23,516 2015-11-20,498 2015-11-19,504 2015-11-18,519 2015-11-17,537 2015-11-16,550 2015-11-13,560 2015-11-12,579 2015-11-11,599 2015-11-10,622 2015-11-09,628 2015-11-06,631 2015-11-05,640 2015-11-04,657 2015-11-03,680 2015-11-02,706 2015-10-30,721 2015-10-29,728 2015-10-28,736 2015-10-27,739 2015-10-26,759 2015-10-23,774 2015-10-22,786 2015-10-21,780 2015-10-20,762 2015-10-19,747 2015-10-16,754 2015-10-15,766 2015-10-14,787 2015-10-13,804 2015-10-12,809 2015-10-09,809 2015-10-08,817 2015-10-07,841 2015-10-06,869 2015-10-05,881 2015-10-02,889 2015-10-01,888 2015-09-30,900 2015-09-29,926 2015-09-28,943 2015-09-25,943 2015-09-24,922 2015-09-23,917 2015-09-22,923 2015-09-21,978 2015-09-18,960 2015-09-17,883 2015-09-16,814 2015-09-15,802 2015-09-14,805 2015-09-11,818 2015-09-10,830 2015-09-09,855 2015-09-08,873 2015-09-07,876 2015-09-04,875 2015-09-03,891 2015-09-02,906 2015-09-01,911 2015-08-28,903 2015-08-27,905 2015-08-26,918 2015-08-25,942 2015-08-24,968 2015-08-21,994 2015-08-20,1014 2015-08-19,1031 2015-08-18,1046 2015-08-17,1063 2015-08-14,1055 2015-08-13,1046 2015-08-12,1093 2015-08-11,1162 2015-08-10,1197 2015-08-07,1200 2015-08-06,1201 2015-08-05,1222 2015-08-04,1200 2015-08-03,1151 2015-07-31,1131 2015-07-30,1100 2015-07-29,1104 2015-07-28,1094 2015-07-27,1090 2015-07-24,1086 2015-07-23,1102 2015-07-22,1118 2015-07-21,1113 2015-07-20,1067 2015-07-17,1048 2015-07-16,1009 2015-07-15,951 2015-07-14,915 2015-07-13,900 2015-07-10,874 2015-07-09,853 2015-07-08,840 2015-07-07,830 2015-07-06,815 2015-07-03,805 2015-07-02,794 2015-07-01,794 2015-06-30,800 2015-06-29,813 2015-06-26,823 2015-06-25,829 2015-06-24,829 2015-06-23,790 2015-06-22,779 2015-06-19,779 2015-06-18,773 2015-06-17,725 2015-06-16,681 2015-06-15,656 2015-06-12,642 2015-06-11,629 2015-06-10,618 2015-06-09,612 2015-06-08,610 2015-06-05,610 2015-06-04,603 2015-06-03,598 2015-06-02,591 2015-06-01,589 2015-05-29,589 2015-05-28,588 2015-05-27,587 2015-05-26,584 2015-05-22,586 2015-05-21,592 2015-05-20,606 2015-05-19,620 2015-05-18,630 2015-05-15,634 2015-05-14,637 2015-05-13,634 2015-05-12,589 2015-05-11,578 2015-05-08,574 2015-05-07,573 2015-05-06,575 2015-05-05,580 2015-05-01,587 2015-04-30,591 2015-04-29,595 2015-04-28,601 2015-04-27,600 2015-04-24,600 2015-04-23,599 2015-04-22,600 2015-04-21,601 2015-04-20,598 2015-04-17,597 2015-04-16,593 2015-04-15,586 2015-04-14,581 2015-04-13,578 2015-04-10,580 2015-04-09,580 2015-04-08,580 2015-04-07,583 2015-04-02,588 2015-04-01,596 2015-03-31,602 2015-03-30,599 2015-03-27,596 2015-03-26,598 2015-03-25,598 2015-03-24,597 2015-03-23,594 2015-03-20,591 2015-03-19,584 2015-03-18,571 2015-03-17,568 2015-03-16,564 2015-03-13,562 2015-03-12,560 2015-03-11,565 2015-03-10,568 2015-03-09,568 2015-03-06,565 2015-03-05,561 2015-03-04,559 2015-03-03,553 2015-03-02,548 2015-02-27,540 2015-02-26,533 2015-02-25,524 2015-02-24,516 2015-02-23,512 2015-02-20,513 2015-02-19,511 2015-02-18,509 2015-02-17,516 2015-02-16,522 2015-02-13,530 2015-02-12,540 2015-02-11,553 2015-02-10,556 2015-02-09,554 2015-02-06,559 2015-02-05,564 2015-02-04,569 2015-02-03,577 2015-02-02,590 2015-01-30,608 2015-01-29,632 2015-01-28,666 2015-01-27,688 2015-01-26,703 2015-01-23,720 2015-01-22,751 2015-01-21,770 2015-01-20,753 2015-01-19,739 2015-01-16,741 2015-01-15,749 2015-01-14,757 2015-01-13,762 2015-01-12,723 2015-01-09,709 2015-01-08,724 2015-01-07,744 2015-01-06,758 2015-01-05,761 2015-01-02,771 2014-12-24,782 2014-12-23,788 2014-12-22,794 2014-12-19,803 2014-12-18,814 2014-12-17,827 2014-12-16,838 2014-12-15,845 2014-12-12,863 2014-12-11,887 2014-12-10,911 2014-12-09,933 2014-12-08,952 2014-12-05,982 2014-12-04,1019 2014-12-03,1079 2014-12-02,1119 2014-12-01,1137 2014-11-28,1153 2014-11-27,1187 2014-11-26,1239 2014-11-25,1313 2014-11-24,1317 2014-11-21,1324 2014-11-20,1332 2014-11-19,1306 2014-11-18,1296 2014-11-17,1264 2014-11-14,1256 2014-11-13,1264 2014-11-12,1327 2014-11-11,1370 2014-11-10,1418 2014-11-07,1437 2014-11-06,1436 2014-11-05,1464 2014-11-04,1484 2014-11-03,1456 2014-10-31,1428 2014-10-30,1424 2014-10-29,1428 2014-10-28,1395 2014-10-27,1285 2014-10-24,1192 2014-10-23,1155 2014-10-22,1136 2014-10-21,1090 2014-10-20,973 2014-10-17,944 2014-10-16,930 2014-10-15,935 2014-10-14,948 2014-10-13,954 2014-10-10,963 2014-10-09,974 2014-10-08,991 2014-10-07,1015 2014-10-06,1029 2014-10-03,1037 2014-10-02,1041 2014-10-01,1055 2014-09-30,1063 2014-09-29,1062 2014-09-26,1049 2014-09-25,1038 2014-09-24,1056 2014-09-23,1073 2014-09-22,1077 2014-09-19,1075 2014-09-18,1089 2014-09-17,1124 2014-09-16,1150 2014-09-15,1173 2014-09-12,1181 2014-09-11,1186 2014-09-10,1197 2014-09-09,1197 2014-09-08,1166 2014-09-05,1155 2014-09-04,1147 2014-09-03,1142 2014-09-02,1149 2014-09-01,1151 2014-08-29,1147 2014-08-28,1119 2014-08-27,1063 2014-08-26,1070 2014-08-22,1088 2014-08-21,1096 2014-08-20,1061 2014-08-19,1040 2014-08-18,1042 2014-08-15,1015 2014-08-14,942 2014-08-13,871 2014-08-12,836 2014-08-11,792 2014-08-08,777 2014-08-07,765 2014-08-06,759 2014-08-05,755 2014-08-04,753 2014-08-01,751 2014-07-31,755 2014-07-30,754 2014-07-29,747 2014-07-28,743 2014-07-25,739 2014-07-24,732 2014-07-23,727 2014-07-22,723 2014-07-21,724 2014-07-18,732 2014-07-17,738 2014-07-16,755 2014-07-15,782 2014-07-14,798 2014-07-11,814 2014-07-10,836 2014-07-09,863 2014-07-08,881 2014-07-07,888 2014-07-04,893 2014-07-03,890 2014-07-02,890 2014-07-01,894 2014-06-30,850 2014-06-27,831 2014-06-26,824 2014-06-25,846 2014-06-24,867 2014-06-23,886 2014-06-20,904 2014-06-19,902 2014-06-18,867 2014-06-17,858 2014-06-16,880 2014-06-13,906 2014-06-12,939 2014-06-11,973 2014-06-10,1004 2014-06-09,999 2014-06-06,989 2014-06-05,977 2014-06-04,959 2014-06-03,948 2014-06-02,934 2014-05-30,934 2014-05-29,940 2014-05-28,954 2014-05-27,973 2014-05-23,964 2014-05-22,966 2014-05-21,988 2014-05-20,1010 2014-05-19,1022 2014-05-16,1027 2014-05-15,1021 2014-05-14,1002 2014-05-13,982 2014-05-12,987 2014-05-09,997 2014-05-08,1008 2014-05-07,1022 2014-05-06,1022 2014-05-02,1017 2014-05-01,993 2014-04-30,943 2014-04-29,949 2014-04-28,961 2014-04-25,967 2014-04-24,962 2014-04-23,956 2014-04-22,939 2014-04-17,930 2014-04-16,936 2014-04-15,970 2014-04-14,989 2014-04-11,1002 2014-04-10,1029 2014-04-09,1061 2014-04-08,1098 2014-04-07,1186 2014-04-04,1205 2014-04-03,1235 2014-04-02,1273 2014-04-01,1316 2014-03-31,1362 2014-03-28,1373 2014-03-27,1412 2014-03-26,1496 2014-03-25,1578 2014-03-24,1602 2014-03-21,1599 2014-03-20,1621 2014-03-19,1570 2014-03-18,1518 2014-03-17,1481 2014-03-14,1477 2014-03-13,1468 2014-03-12,1453 2014-03-11,1580 2014-03-10,1562 2014-03-07,1543 2014-03-06,1480 2014-03-05,1391 2014-03-04,1325 2014-03-03,1276 2014-02-28,1258 2014-02-27,1250 2014-02-26,1222 2014-02-25,1197 2014-02-24,1174 2014-02-21,1175 2014-02-20,1164 2014-02-19,1160 2014-02-18,1146 2014-02-17,1130 2014-02-14,1106 2014-02-13,1097 2014-02-12,1085 2014-02-11,1091 2014-02-10,1096 2014-02-07,1091 2014-02-06,1092 2014-02-05,1086 2014-02-04,1084 2014-02-03,1093 2014-01-31,1110 2014-01-30,1127 2014-01-29,1148 2014-01-28,1177 2014-01-27,1217 2014-01-24,1246 2014-01-23,1271 2014-01-22,1322 2014-01-21,1369 2014-01-20,1428 2014-01-17,1421 2014-01-16,1398 2014-01-15,1374 2014-01-14,1370 2014-01-13,1395 2014-01-10,1512 2014-01-09,1706 2014-01-08,1826 2014-01-07,1876 2014-01-06,1951 2014-01-03,2036 2014-01-02,2113 2013-12-23,2247 2013-12-20,2208 2013-12-19,2134 2013-12-18,2156 2013-12-17,2225 2013-12-16,2292 2013-12-13,2330 2013-12-12,2337 2013-12-11,2299 2013-12-10,2237 2013-12-09,2183 2013-12-06,2176 2013-12-05,2145 2013-12-04,1994 2013-12-03,1922 2013-12-02,1865 2013-11-29,1821 2013-11-28,1719 2013-11-27,1573 2013-11-26,1512 2013-11-25,1492 2013-11-22,1483 2013-11-21,1499 2013-11-20,1527 2013-11-19,1495 2013-11-18,1500 2013-11-15,1507 2013-11-14,1517 2013-11-13,1531 2013-11-12,1543 2013-11-11,1564 2013-11-08,1581 2013-11-07,1593 2013-11-06,1602 2013-11-05,1600 2013-11-04,1552 2013-11-01,1525 2013-10-31,1504 2013-10-30,1484 2013-10-29,1551 2013-10-28,1619 2013-10-25,1671 2013-10-24,1708 2013-10-23,1786 2013-10-22,1847 2013-10-21,1878 2013-10-18,1901 2013-10-17,1960 2013-10-16,1965 2013-10-15,1963 2013-10-14,1961 2013-10-11,1985 2013-10-10,2011 2013-10-09,2125 2013-10-08,2146 2013-10-07,2115 2013-10-04,2084 2013-10-03,2047 2013-10-02,2008 2013-10-01,1994 2013-09-30,2003 2013-09-27,2046 2013-09-26,2113 2013-09-25,2127 2013-09-24,2021 2013-09-23,1947 2013-09-20,1904 2013-09-19,1860 2013-09-18,1822 2013-09-17,1740 2013-09-16,1651 2013-09-13,1636 2013-09-12,1621 2013-09-11,1628 2013-09-10,1541 2013-09-09,1478 2013-09-06,1352 2013-09-05,1279 2013-09-04,1215 2013-09-03,1168 2013-09-02,1139 2013-08-30,1132 2013-08-29,1136 2013-08-28,1146 2013-08-27,1169 2013-08-23,1165 2013-08-22,1158 2013-08-21,1156 2013-08-20,1145 2013-08-19,1115 2013-08-16,1102 2013-08-15,1091 2013-08-14,1060 2013-08-13,1007 2013-08-12,996 2013-08-09,1001 2013-08-08,1012 2013-08-07,1024 2013-08-06,1046 2013-08-05,1058 2013-08-02,1065 2013-08-01,1066 2013-07-31,1062 2013-07-30,1067 2013-07-29,1075 2013-07-25,1092 2013-07-23,1127 2013-07-22,1135 2013-07-19,1138 2013-07-18,1146 2013-07-17,1151 2013-07-16,1152 2013-07-15,1151 2013-07-12,1149 2013-07-11,1139 2013-07-10,1130 2013-07-09,1120 2013-07-08,1115 2013-07-03,1133 2013-07-02,1170 2013-07-01,1179 2013-06-28,1171 2013-06-27,1151 2013-06-26,1125 2013-06-25,1090 2013-06-24,1062 2013-06-21,1027 2013-06-20,1012 2013-06-19,995 2013-06-18,962 2013-06-17,925 2013-06-14,900 2013-06-13,873 2013-06-12,847 2013-06-11,825 2013-06-10,815 2013-06-07,812 2013-06-06,806 2013-06-05,801 2013-06-04,805 2013-06-03,806 2013-05-31,809 2013-05-30,811 2013-05-29,818 2013-05-28,822 2013-05-24,826 2013-05-23,828 2013-05-22,829 2013-05-21,830 2013-05-20,836 2013-05-17,841 2013-05-16,850 2013-05-15,861 2013-05-14,872 2013-05-13,879 2013-05-10,884 2013-05-09,889 2013-05-08,892 2013-05-07,889 2013-05-03,878 2013-05-02,873 2013-05-01,862 2013-04-30,863 2013-04-29,868 2013-04-26,871 2013-04-25,872 2013-04-24,879 2013-04-23,885 2013-04-22,889 2013-04-19,888 2013-04-18,885 2013-04-17,885 2013-04-16,880 2013-04-15,876 2013-04-12,875 2013-04-11,865 2013-04-10,859 2013-04-09,856 2013-04-08,858 2013-04-05,861 2013-04-04,866 2013-04-03,877 2013-04-02,896 2013-03-28,910 2013-03-27,922 2013-03-26,931 2013-03-25,935 2013-03-22,933 2013-03-21,930 2013-03-20,923 2013-03-19,912 2013-03-18,899 2013-03-15,892 2013-03-14,880 2013-03-13,875 2013-03-12,865 2013-03-11,847 2013-03-08,843 2013-03-07,834 2013-03-06,820 2013-03-05,806 2013-03-04,789 2013-03-01,776 2013-02-28,757 2013-02-27,745 2013-02-26,741 2013-02-25,743 2013-02-22,740 2013-02-21,737 2013-02-20,735 2013-02-19,738 2013-02-18,747 2013-02-15,753 2013-02-14,748 2013-02-13,751 2013-02-12,747 2013-02-11,746 2013-02-08,748 2013-02-07,749 2013-02-06,740 2013-02-05,739 2013-02-04,745 2013-02-01,750 2013-01-31,760 2013-01-30,767 2013-01-29,779 2013-01-28,792 2013-01-25,798 2013-01-24,808 2013-01-23,817 2013-01-22,825 2013-01-21,838 2013-01-18,837 2013-01-17,820 2013-01-16,781 2013-01-15,765 2013-01-14,762 2013-01-11,760 2013-01-10,751 2013-01-09,743 2013-01-08,734 2013-01-07,712 2013-01-04,706 2013-01-03,700 2013-01-02,698 2012-12-21,700 2012-12-20,708 2012-12-19,720 2012-12-18,743 2012-12-17,766 2012-12-14,784 2012-12-13,799 2012-12-12,826 2012-12-11,900 2012-12-10,937 2012-12-07,966 2012-12-06,990 2012-12-05,1022 2012-12-04,1054 2012-12-03,1077 2012-11-30,1086 2012-11-29,1097 2012-11-28,1104 2012-11-27,1097 2012-11-26,1094 2012-11-23,1090 2012-11-22,1084 2012-11-21,1073 2012-11-20,1066 2012-11-19,1054 2012-11-16,1036 2012-11-15,1024 2012-11-14,1011 2012-11-13,985 2012-11-12,965 2012-11-09,940 2012-11-08,916 2012-11-07,916 2012-11-06,947 2012-11-05,971 2012-11-02,986 2012-11-01,1000 2012-10-31,1026 2012-10-30,1043 2012-10-29,1048 2012-10-26,1049 2012-10-25,1051 2012-10-24,1088 2012-10-23,1109 2012-10-22,1037 2012-10-19,1010 2012-10-18,989 2012-10-17,999 2012-10-16,981 2012-10-15,941 2012-10-12,926 2012-10-11,903 2012-10-10,875 2012-10-09,875 2012-10-08,883 2012-10-05,875 2012-10-04,845 2012-10-03,798 2012-10-02,778 2012-10-01,777 2012-09-28,766 2012-09-27,744 2012-09-26,752 2012-09-25,763 2012-09-24,772 2012-09-21,774 2012-09-20,755 2012-09-19,722 2012-09-18,697 2012-09-17,663 2012-09-14,662 2012-09-13,663 2012-09-12,661 2012-09-11,662 2012-09-10,666 2012-09-07,669 2012-09-06,675 2012-09-05,684 2012-09-04,693 2012-09-03,698 2012-08-31,703 2012-08-30,707 2012-08-29,718 2012-08-28,724 2012-08-24,717 2012-08-23,715 2012-08-22,712 2012-08-21,709 2012-08-20,711 2012-08-17,714 2012-08-16,720 2012-08-15,728 2012-08-14,750 2012-08-13,764 2012-08-10,774 2012-08-09,790 2012-08-08,812 2012-08-07,836 2012-08-06,843 2012-08-03,852 2012-08-02,861 2012-08-01,878 2012-07-31,897 2012-07-30,915 2012-07-27,933 2012-07-26,958 2012-07-25,982 2012-07-24,1003 2012-07-23,1022 2012-07-20,1037 2012-07-19,1053 2012-07-18,1074 2012-07-17,1093 2012-07-16,1102 2012-07-13,1110 2012-07-12,1121 2012-07-11,1146 2012-07-10,1160 2012-07-09,1162 2012-07-06,1157 2012-07-05,1138 2012-07-04,1103 2012-07-03,1063 2012-07-02,1013 2012-06-29,1004 2012-06-28,994 2012-06-27,988 2012-06-26,981 2012-06-25,978 2012-06-22,978 2012-06-21,978 2012-06-20,972 2012-06-19,954 2012-06-18,938 2012-06-15,924 2012-06-14,912 2012-06-13,902 2012-06-12,893 2012-06-11,884 2012-06-08,877 2012-06-07,872 2012-06-06,878 2012-06-01,904 2012-05-31,923 2012-05-30,950 2012-05-29,986 2012-05-28,1012 2012-05-25,1034 2012-05-24,1058 2012-05-23,1100 2012-05-22,1127 2012-05-21,1141 2012-05-18,1141 2012-05-17,1137 2012-05-16,1137 2012-05-15,1130 2012-05-14,1132 2012-05-11,1138 2012-05-10,1146 2012-05-09,1156 2012-05-08,1165 2012-05-04,1157 2012-05-03,1157 2012-05-02,1149 2012-05-01,1152 2012-04-30,1155 2012-04-27,1156 2012-04-26,1148 2012-04-25,1137 2012-04-24,1116 2012-04-23,1090 2012-04-20,1067 2012-04-19,1028 2012-04-18,1006 2012-04-17,989 2012-04-16,975 2012-04-13,972 2012-04-12,960 2012-04-11,944 2012-04-10,928 2012-04-05,928 2012-04-04,926 2012-04-03,931 2012-04-02,934 2012-03-30,934 2012-03-29,930 2012-03-28,922 2012-03-27,917 2012-03-26,912 2012-03-23,908 2012-03-22,902 2012-03-21,896 2012-03-20,884 2012-03-19,879 2012-03-16,874 2012-03-15,866 2012-03-14,855 2012-03-13,844 2012-03-12,837 2012-03-09,824 2012-03-08,812 2012-03-07,798 2012-03-06,787 2012-03-05,782 2012-03-02,771 2012-03-01,763 2012-02-29,750 2012-02-28,738 2012-02-27,730 2012-02-24,718 2012-02-23,706 2012-02-22,704 2012-02-21,706 2012-02-20,715 2012-02-17,717 2012-02-16,723 2012-02-15,731 2012-02-14,734 2012-02-13,715 2012-02-10,715 2012-02-09,695 2012-02-08,676 2012-02-07,660 2012-02-06,648 2012-02-03,647 2012-02-02,651 2012-02-01,662 2012-01-31,680 2012-01-30,702 2012-01-27,726 2012-01-26,753 2012-01-25,784 2012-01-24,807 2012-01-23,841";

            return chartData;
        }
    }

   
   }

