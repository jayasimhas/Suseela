

// THIS FILE IS DEPRECATED
// SEE: Views/Shared/Global/Analytics.cshtml


// ﻿function getCorrespondingSectr(sector) {
//     if (sector == 'comment' || sector == 'features' || sector == 'analysis') {
//         sector = 'analysis';
//     } else if (sector == 'supplementsreports' || sector == 'multimedia' || sector == 'scienceabstracts') {
//         sector = 'home';
//     } else if (sector == 'therapysector' || sector == 'other') {
//         sector = 'therapy';
//     } else if (sector == 'cardiovascular') {
//         sector = 'cardio';
//     } else if (sector == 'musculoskeletal') {
//         sector = 'musculo';
//     } else if (sector == 'gastrointestinal') {
//         sector = 'gastro';
//     } else if (sector == 'genericsbusiness') {
//         sector = 'genbusiness';
//     } else if (sector == 'manda') {
//         sector = 'merger';
//     } else if (sector == 'businessstrategy') {
//         sector = 'corp';
//     } else if (sector == 'lifecyclemanagement') {
//         sector = 'lifecycle';
//     } else if (sector == 'policyregulation') {
//         sector = 'policyandreg';
//     } else if (sector == 'pricingreimbursement') {
//         sector = 'pandr';
//     } else if (sector == 'healthcarepolicy') {
//         sector = 'hpolicy';
//     } else if (sector == 'pharmacovigilance') {
//         sector = 'pharmacv';
//     } else if (sector == 'intellectualproperty') {
//         sector = 'ip';
//     } else if (sector == 'researchdevelopment') {
//         sector = 'randd';
//     } else if (sector == 'productapprovals') {
//         sector = 'approvals';
//     } else if (sector == 'clinicaltrials') {
//         sector = 'ctrials';
//     } else if (sector == 'productfilings') {
//         sector = 'filings';
//     } else if (sector == 'productsafety') {
//         sector = 'safety';
//     } else if (sector == 'productdelays') {
//         sector = 'delays';
//     } else if (sector == 'latinamerica') {
//         sector = 'lamerica';
//     } else if (sector == 'world') {
//         sector = 'row';
//     }
//     return sector;
// }
//
// //configuration
// OAS_url = 'https://oasc-eu1.247realmedia.com/RealMedia/ads/';
// OAS_sitepage = 'www.scripnews.com/' + getCorrespondingSectr('home');
// OAS_listpos = 'Top,Top1,Bottom,Right,Right1,TopRight,Top2,Top3,Middle,Middle1,Position2,Position3';
// OAS_query = '';
// OAS_target = 'ad';
// //end of configuration
// OAS_version = 11;
// OAS_rn = '001234567890';
// OAS_rns = '1234567890';
// OAS_rn = new String (Math.random()); OAS_rns = OAS_rn.substring (2, 11);
//
// function OAS_NORMAL(pos,sector,keyword) {
//     document.write('<A HREF="' + OAS_url + 'click_nx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + '!' + pos + OAS_query + '" TARGET="' + OAS_target + '">');
//     document.write('<IMG SRC="' + OAS_url + 'adstream_nx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + '!' + pos + OAS_query + '" BORDER=0 ALT="Advertisement" Title=""></a>');
// }
//
// if (navigator.userAgent.indexOf('Mozilla/3') != -1)
//     OAS_version = 10;
// if (OAS_version >= 11)
//     document.write('<SC'+'RIPT LANGUAGE="JavaScript1.1" SRC="' + OAS_url + 'adstream_mjx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + OAS_query + '"><\/SCRIPT>');
//
// document.write('');
// function OAS_AD(pos,sector) {
//     if (OAS_version >= 11 && typeof(OAS_RICH)!='undefined')
//         OAS_RICH(pos);
//     else
//         OAS_NORMAL(pos,sector,'');
// }
// function OAS_AD_SEARCH(pos,sector,keyword) {
//
//     var sectorName = getCorrespondingSectr(sector);
//
//     if (OAS_version >= 11 && typeof(OAS_RICH)!='undefined'){
//         OAS_RICH(pos);
//     }
//     else{
//         OAS_NORMAL(pos,sectorName,keyword);
//     }
// }
