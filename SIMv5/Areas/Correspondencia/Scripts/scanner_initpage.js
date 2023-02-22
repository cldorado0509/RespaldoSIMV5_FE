var _iLeft, _iTop, _iRight, _iBottom; //These variables are used to remember the selected area
Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', Dynamsoft_OnReady);

var DWObject;

function pageonload() {

    Dynamsoft.WebTwainEnv.ResourcesPath = '/SIMV5/Resources';
    Dynamsoft.WebTwainEnv.Containers = [{ ContainerId: 'dwtcontrolContainer', Width: 1000, Height: 450 }];

    initMessageBox(false);  //Messagebox
    //initCustomScan();       //CustomScan

    initiateInputs();
}

function AcquireImage() {
    if (DWObject) {
        var OnAcquireImageSuccess, OnAcquireImageFailure;
        OnAcquireImageSuccess = OnAcquireImageFailure = function () {
            DWObject.CloseSource();
        };

        DWObject.SelectSourceByIndex(document.getElementById("source").selectedIndex); //Use method SelectSourceByIndex to avoid the 'Select Source' dialog
        DWObject.OpenSource();
        DWObject.IfDisableSourceAfterAcquire = true;	// Scanner source will be disabled/closed automatically after the scan.
        DWObject.AcquireImage(OnAcquireImageSuccess, OnAcquireImageFailure);
    }
}

function OnSuccess() {
    console.log('successful');
}

function OnFailure(errorCode, errorString) {
    alert(errorString);
}

function LoadImage() {
    if (DWObject) {
        DWObject.IfShowFileDialog = true; // Open the system's file dialog to load image
        DWObject.LoadImageEx("", EnumDWT_ImageType.IT_ALL, OnSuccess, OnFailure); // Load images in all supported formats (.bmp, .jpg, .tif, .png, .pdf). OnSuccess or OnFailure will be called after the operation
    }
}

function initMessageBox(bNeebBack) {
    /*var objString = "";

    // The container for navigator, view mode and remove button
    objString += "<div style='text-align:center; width:580px; background-color:#FFFFFF;display:block'>";
    objString += "<div style='position:relative; background:white; float:left; width:430px; height:35px;'>";
    objString += "<input id='DW_btnFirstImage' onclick='btnFirstImage_onclick()' type='button' value=' |&lt; '/>&nbsp;";
    objString += "<input id='DW_btnPreImage' onclick='btnPreImage_onclick()' type='button' value=' &lt; '/>&nbsp;&nbsp;";
    objString += "<input type='text' size='2' id='DW_CurrentImage' readonly='readonly'/>/";
    objString += "<input type='text' size='2' id='DW_TotalImage' readonly='readonly'/>&nbsp;&nbsp;";
    objString += "<input id='DW_btnNextImage' onclick='btnNextImage_onclick()' type='button' value=' &gt; '/>&nbsp;";
    objString += "<input id='DW_btnLastImage' onclick='btnLastImage_onclick()' type='button' value=' &gt;| '/></div>";
    objString += "<div style='position:relative; background:white; float:left; width:150px; height:35px;'>Preview Mode";
    objString += "<select size='1' id='DW_PreviewMode' onchange ='setlPreviewMode();'>";
    objString += "    <option value='0'>1X1</option>";
    objString += "</select><br /></div>";
    objString += "<div><input id='DW_btnRemoveCurrentImage' onclick='btnRemoveCurrentImage_onclick()' type='button' value='Eliminar Imagen'/>";
    objString += "<input id='DW_btnRemoveAllImages' onclick='btnRemoveAllImages_onclick()' type='button' value='Eliminar Todas las Im&aacute;ges'/><br /></div>";
    objString += "</div>";

    // The container for the error message
    objString += "<div id='DWTdivMsg' style='width:580px;display:inline'>";
    //objString += "Message:<br />"
    //objString += "<div id='DWTemessage' style='width:580px;height:80px; overflow:scroll; background-color:#ffffff; border:1px #303030; border-style:solid; text-align:left; position:relative' >";
    //objString += "</div></div>";

    var DWTemessageContainer = document.getElementById("DWTemessageContainer");
    DWTemessageContainer.innerHTML = objString;*/

    // Fill the init data for preview mode selection
    //var varPreviewMode = document.getElementById("DW_PreviewMode");
    //varPreviewMode.options.length = 0;
    //varPreviewMode.options.add(new Option("1X1", 0));
    //varPreviewMode.options.add(new Option("2X2", 1));
    //varPreviewMode.options.add(new Option("3X3", 2));
    //varPreviewMode.options.add(new Option("4X4", 3));
    //varPreviewMode.options.add(new Option("5X5", 4));
    //varPreviewMode.selectedIndex = 0;
    //varPreviewMode.selectedIndex = 1;

    /*var _divMessageContainer = document.getElementById("DWTemessage");
    _divMessageContainer.ondblclick = function() {
        this.innerHTML = "";
        _strTempStr = "";
    }*/

}

function initiateInputs() {

    var allinputs = document.getElementsByTagName("input");
    for (var i = 0; i < allinputs.length; i++) {
        if (allinputs[i].type == "checkbox") {
            allinputs[i].checked = false;
        }
        else if (allinputs[i].type == "text") {
            allinputs[i].value = "";
        }
    }

    if (!Dynamsoft.Lib.env.bWin) {
        document.getElementById("btnEditor").style.display = "none";
        document.getElementById("tblLoadImage").style.height = "170";
        document.getElementById("notformac1").style.display = "none";
    }

    if (Dynamsoft.Lib.env.bIE == true && Dynamsoft.Lib.env.bWin64 == true) {
        document.getElementById("samplesource64bit").style.display = "inline";
        document.getElementById("samplesource32bit").style.display = "none";
    }
}



function initDllForChangeImageSize() {

    /*var vInterpolationMethod = document.getElementById("InterpolationMethod");
    vInterpolationMethod.options.length = 0;
    vInterpolationMethod.options.add(new Option("NearestNeighbor", 1));
    vInterpolationMethod.options.add(new Option("Bilinear", 2));
    vInterpolationMethod.options.add(new Option("Bicubic", 3));*/

}

function setDefaultValue() {
    document.getElementById("Gray").checked = true;
 
    var varImgTypejpeg2 = document.getElementById("imgTypejpeg2");
    if (varImgTypejpeg2)
        varImgTypejpeg2.checked = true;
    var varImgTypejpeg = document.getElementById("imgTypejpeg");
    if (varImgTypejpeg)
        varImgTypejpeg.checked = true;

    /*var _strDefaultSaveImageName = "WebTWAINImage";
    var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
    _txtFileNameforSave.value = _strDefaultSaveImageName;

    var _txtFileName = document.getElementById("txt_fileName");
    _txtFileName.value = _strDefaultSaveImageName;

    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    _chkMultiPageTIFF_save.disabled = true;
    var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
    _chkMultiPagePDF_save.disabled = true;
    var _chkMultiPageTIFF = document.getElementById("MultiPageTIFF");
    _chkMultiPageTIFF.disabled = true;
    var _chkMultiPagePDF = document.getElementById("MultiPagePDF");
    _chkMultiPagePDF.disabled = true;*/
}


var DWObject;            // The DWT Object
// Check if the control is fully loaded.
function Dynamsoft_OnReady() {

        
    var liNoScanner = document.getElementById("pNoScanner");

    // If the ErrorCode is 0, it means everything is fine for the control. It is fully loaded.
    DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');  
    if (DWObject) {
        if (DWObject.ErrorCode == 0) {                
            DWObject.LogLevel = 0;
            DWObject.IfAllowLocalCache = true;

            if (!document.getElementById("source"))
                return;

            document.getElementById("source").options.length = 0;
            var vCount = DWObject.SourceCount;
            // If source list need to be displayed, fill in the source items.
            if (vCount == 0) {

                if (Dynamsoft.Lib.env.bWin) {
                    liNoScanner.style.display = "block";
                    liNoScanner.style.textAlign = "center";
                }
                else
                    liNoScanner.style.display = "none";
            }

           
            for (var i = 0; i < vCount; i++) {
                document.getElementById("source").options.add(new Option(DWObject.GetSourceNameItems(i), i));
            }

            if (vCount > 0) {
                source_onchange();
            } 

            if (Dynamsoft.Lib.env.bWin)
                DWObject.MouseShape = false;

            if (!Dynamsoft.Lib.env.bWin && DWObject.ImageCaptureDriverType != 0) {
                if (document.getElementById("lblShowUI"))
                    document.getElementById("lblShowUI").style.display = "none";
                if (document.getElementById("ShowUI"))
                document.getElementById("ShowUI").style.display = "none";
            }
            else {
                if(document.getElementById("lblShowUI"))
                    document.getElementById("lblShowUI").style.display = "";
                if (document.getElementById("ShowUI"))
                    document.getElementById("ShowUI").style.display = "";
            }

            initDllForChangeImageSize();

            re = /^\d+$/;
            strre = /^[\s\w]+$/;
            refloat = /^\d+\.*\d*$/i;

            _iLeft = 0;
            _iTop = 0;
            _iRight = 0;
            _iBottom = 0;

            for (var i = 0; i < document.links.length; i++) {
                if (document.links[i].className == "ShowtblLoadImage") {
                    document.links[i].onclick = showtblLoadImage_onclick;
                }
                if (document.links[i].className == "ClosetblLoadImage") {
                    document.links[i].onclick = closetblLoadImage_onclick;
                }
            }
            if (vCount == 0) {
                if (Dynamsoft.Lib.env.bWin) {
                    document.getElementById("aNoScanner").style.color = "Red";
                    document.getElementById("aNoScanner").innerHTML = "<b>No TWAIN compatible drivers detected:<b/>";
                    document.getElementById("Resolution").style.display = "none";
                    showtblLoadImage_onclick();
                }

            }
            //else
                //document.getElementById("divBlank").style.display = "none";

            updatePageInfo();
            ua = (navigator.userAgent.toLowerCase());
            if (!ua.indexOf("msie 6.0")) {
                ShowSiteTour();
            }

            DWObject.RegisterEvent("OnPostTransfer", Dynamsoft_OnPostTransfer);
            DWObject.RegisterEvent("OnPostLoad", Dynamsoft_OnPostLoadfunction);
            DWObject.RegisterEvent("OnPostAllTransfers", Dynamsoft_OnPostAllTransfers);
            DWObject.RegisterEvent("OnMouseClick", Dynamsoft_OnMouseClick);            
            DWObject.RegisterEvent("OnImageAreaSelected", Dynamsoft_OnImageAreaSelected);
            DWObject.RegisterEvent("OnImageAreaDeSelected", Dynamsoft_OnImageAreaDeselected);
            DWObject.RegisterEvent("OnTopImageInTheViewChanged", Dynamsoft_OnTopImageInTheViewChanged);

        }
        setlPreviewModeFixed();
    }
}


function showtblLoadImage_onclick() {
    switch (document.getElementById("tblLoadImage").style.visibility) {
        case "hidden": document.getElementById("tblLoadImage").style.visibility = "visible";
            document.getElementById("Resolution").style.visibility = "hidden";
            break;
        case "visible":
            document.getElementById("tblLoadImage").style.visibility = "hidden";
            document.getElementById("Resolution").style.visibility = "visible";
            break;
        default: break;
    }
    document.getElementById("tblLoadImage").style.top = ds_gettop(document.getElementById("pNoScanner")) + pNoScanner.offsetHeight + "px";
    document.getElementById("tblLoadImage").style.left = ds_getleft(document.getElementById("pNoScanner")) + 0 + "px";
    return false;
}

function closetblLoadImage_onclick() {
    document.getElementById("tblLoadImage").style.visibility = "hidden";
    document.getElementById("Resolution").style.visibility = "visible";
    return false;
}

//--------------------------------------------------------------------------------------
//************************** Used a lot *****************************
//--------------------------------------------------------------------------------------
function updatePageInfo() {
    document.getElementById("DW_TotalImage").value = DWObject.HowManyImagesInBuffer;
    document.getElementById("DW_CurrentImage").value = DWObject.CurrentImageIndexInBuffer + 1;
}


var _strTempStr = "";       // Store the temp string for display
function appendMessage(strMessage) {
    _strTempStr += strMessage;
    var _divMessageContainer = document.getElementById("DWTemessage");
    if (_divMessageContainer) {
        _divMessageContainer.innerHTML = _strTempStr;
        _divMessageContainer.scrollTop = _divMessageContainer.scrollHeight;
    }
}

function checkIfImagesInBuffer() {
    if (DWObject.HowManyImagesInBuffer == 0) {
        appendMessage("There is no image in buffer.<br />")
        return false;
    }
    else
        return true;
}

function checkErrorString() {
    return checkErrorStringWithErrorCode(DWObject.ErrorCode, DWObject.ErrorString);
}

function checkErrorStringWithErrorCode(errorCode, errorString, responseString) {
    if (errorCode == 0) {
        appendMessage("<span style='color:#cE5E04'><b>" + errorString + "</b></span><br />");

        return true;
    }
    if (errorCode == -2115) //Cancel file dialog
        return true;
    else {
        if (errorCode == -2003) {
            var ErrorMessageWin = window.open("", "ErrorMessage", "height=500,width=750,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
            ErrorMessageWin.document.writeln(responseString); //DWObject.HTTPPostResponseString);
        }
        appendMessage("<span style='color:#cE5E04'><b>" + errorString + "</b></span><br />");
        return false;
    }
}


//--------------------------------------------------------------------------------------
//************************** Used a lot *****************************
//--------------------------------------------------------------------------------------
function ds_getleft(el) {
    var tmp = el.offsetLeft;
    el = el.offsetParent
    while (el) {
        tmp += el.offsetLeft;
        el = el.offsetParent;
    }
    return tmp;
}
function ds_gettop(el) {
    var tmp = el.offsetTop;
    el = el.offsetParent
    while (el) {
        tmp += el.offsetTop;
        el = el.offsetParent;
    }
    return tmp;
}

function Over_Out_DemoImage(obj, url) {
    obj.src = url;
}


window.onload = function() {
    document.onscroll = MouseScroll;
}

function MouseScroll(evt) {
    var mousewheelevt = (/Firefox/i.test(navigator.userAgent)) ? "DOMMouseScroll" : "mousewheel";
    if (document.attachEvent)
        document.attachEvent("on" + mousewheelevt, NavigateImages);
    else if (document.addEventListener);
    document.addEventListener(mousewheelevt, NavigateImages, false)
}

function NavigateImages(e) {
    evt = window.event || e;
    var delta = evt.detail ? evt.detail * (-120) : evt.wheelDelta;
    if (delta < 0)
        btnNextImage_wheel();
    else if (delta > 0)
        btnPreImage_wheel();
}

function stopWheel(evt) {
    if (!evt) { /* IE7, IE8, Chrome, Safari */
        evt = window.event;
    }
    if (evt.preventDefault) { /* Chrome, Safari, Firefox */
        var ret = evt.preventDefault();
    }
    evt.returnValue = false; /* IE7, IE8 */
}


