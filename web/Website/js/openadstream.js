
//configuration
OAS_url = 'http://oas.informahealthcare.com/RealMedia/ads/';
OAS_listpos = 'TopRight,Top1,Top2,Top3,Middle,Position2,Position3,Middle1,Right';
OAS_query = '';
OAS_target = 'ad';
//end of configuration
OAS_version = 11;
OAS_rn = '001234567890';
OAS_rns = '1234567890';
OAS_rn = new String (Math.random()); OAS_rns = OAS_rn.substring (2, 11);

function OAS_NORMAL(pos,sector,keyword) {
    document.write('<A HREF="' + OAS_url + 'click_nx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + '!' + pos + OAS_query + '" TARGET="' + OAS_target + '">');
    document.write('<IMG SRC="' + OAS_url + 'adstream_nx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + '!' + pos + OAS_query + '" BORDER=0 ALT="Advertisement" Title=""></a>');
}

if (navigator.userAgent.indexOf('Mozilla/3') != -1)
    OAS_version = 10;
if (OAS_version >= 11)
    document.write('<SC'+'RIPT LANGUAGE="JavaScript1.1" SRC="' + OAS_url + 'adstream_mjx.ads/' + OAS_sitepage + '/1' + OAS_rns + '@' + OAS_listpos + OAS_query + '"><\/SCRIPT>');

document.write('');
function OAS_AD(pos,sector) {
    if (OAS_version >= 11 && typeof(OAS_RICH)!='undefined')
        OAS_RICH(pos);
    else
        OAS_NORMAL(pos,sector,'');
}
function OAS_AD_SEARCH(pos,sector,keyword) {

    var sectorName = getCorrespondingSectr(sector);

    if (OAS_version >= 11 && typeof(OAS_RICH)!='undefined'){
        OAS_RICH(pos);
    }
    else{
        OAS_NORMAL(pos,sectorName,keyword);
    }
}