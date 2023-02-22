$(window).resize(function() {
   ResponsiveMenu()
});
 $(function() {
      $("body").click(function() {
   
        if($("#menuAMVAMini")[0].className= "dlMini-menu dlMini-menuopen" ){
          $(".dlMini-trigger").click();
        }                 
      });
  
  });
         
function ResponsiveMenu() {
   if ($(window).width() < 789) {
     $("#menuAMVA").css("height", "50px")
       $("#dlMini-menu").css("display", "block")
       $("#dl-menu").css("display", "none")

   } else {
     if($("#menuAMVAMini")[0].className= "dlMini-menu dlMini-menuopen dlMini-menu-toggle" ){
          $(".dlMini-trigger").click();
        }                 
       $("#dlMini-menu").css("display", "none")
       $("#dl-menu").css("display", "block")
   }
}

function CrearMenu(jsonA) {
   var DatosMenu = eval('(' + jsonA + ')');
   var html = '<ul class="dl-menu" id="menuAMVA">'
   for (var i = 0; i < DatosMenu.length; i++) {
       var url = "javascript:void(0)"
       if (DatosMenu[i].URL != "") {
           url = DatosMenu[i].URL
       }
       html += '  <li><a href="' + url + '" TARGET="_blank">' + DatosMenu[i].NOMBRE + '</a>'
       if (DatosMenu[i].MENU.length != 0) {
           html += '<ul class="dl-submenu">'
           for (var s1 = 0; s1 < DatosMenu[i].MENU.length; s1++) {
               var url = "javascript:void(0)"
               if (DatosMenu[i].MENU[s1].URL != "") {
                   url = DatosMenu[i].MENU[s1].URL
               }
               html += '   <li><a href="' + url + '" TARGET="_blank">' + DatosMenu[i].MENU[s1].NOMBRE + '</a>'
                   ///////////////////////////////////////////
               if (DatosMenu[i].MENU[s1].MENU.length != 0) {
                   html += '<ul class="dl-submenu">'
                   for (var s2 = 0; s2 < DatosMenu[i].MENU[s1].MENU.length; s2++) {
                       var url = "javascript:void(0)"
                       if (DatosMenu[i].MENU[s1].MENU[s2].URL != "") {
                           url = DatosMenu[i].MENU[s1].MENU[s2].URL
                       }
                       html += ' <li><a href="' + url + '" TARGET="_blank">' + DatosMenu[i].MENU[s1].MENU[s2].NOMBRE + '</a>'
                           ///////////////////////////////////////////
                       if (DatosMenu[i].MENU[s1].MENU[s2].MENU.length != 0) {
                           html += '<ul class="dl-submenu">'
                           for (var s3 = 0; s3 < DatosMenu[i].MENU[s1].MENU[s2].MENU.length; s3++) {
                               var url = "javascript:void(0)"
                               if (DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].URL != "") {
                                   url = DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].URL
                               }
                               html += '  <li><a href="' + url + '" TARGET="_blank">' + DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].NOMBRE + '</a>'
                                   ///////////////////////////////////////////
                               if (DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].MENU.length != 0) {
                                   html += '<ul class="dl-submenu">'
                                   for (var s4 = 0; s4 < DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].MENU.length; s4++) {
                                       var url = "javascript:void(0)"
                                       if (DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].MENU[s4].URL != "") {
                                           url = DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].MENU[s4].URL
                                       }
                                       html += '   <li><a href="' + url + '" TARGET="_blank">' + DatosMenu[i].MENU[s1].MENU[s2].MENU[s3].MENU[s4].NOMBRE + '</a>'
                                       html += '   </li>'
                                   };
                                   html += '</ul>'

                               }
                               ///////////////////////////////////////////
                               html += '  </li>'
                           };
                           html += '</ul>'

                       }
                       ///////////////////////////////////////////
                       html += ' </li>'
                   };
                   html += '</ul>'

               }
               ///////////////////////////////////////////
               html += '   </li>'
           };
           html += '</ul>'

       }

       html += '  </li>'
   };
   html += '</ul>'
   try {
       $("#menuAMVA").remove();
   } catch (e) {

   }
   $("#dl-menu").append(html);
   $('#dl-menu').MenuAmva();
   var htmlMini = html.replace(/dl-/g, "dlMini-")
   htmlMini = htmlMini.replace("menuAMVA", "menuAMVAMini")

   $("#dlMini-menu").append(htmlMini);
   $('#dlMini-menu').MenuAmvaMini();
   ResponsiveMenu()
}