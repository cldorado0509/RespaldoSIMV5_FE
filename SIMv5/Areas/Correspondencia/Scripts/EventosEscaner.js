Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', Dynamsoft_OnReady); // Register OnWebTwainReady event. This event fires as soon as Dynamic Web TWAIN is initialized and ready to be used

var DWObject;

function Dynamsoft_OnReady() {
    DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer'); // Get the Dynamic Web TWAIN object that is embeded in the div with id 'dwtcontrolContainer'
    if (DWObject) {
        var count = DWObject.SourceCount; // Populate how many sources are installed in the system
        for (var i = 0; i < count; i++)
            document.getElementById("source").options.add(new Option(DWObject.GetSourceNameItems(i), i));  // Add the sources in a drop-down list

        DWObject.RegisterEvent("OnPostTransfer", function () {
            updatePageInfo();
        });

        // The event OnPostLoad fires after the images from a local directory have been loaded into the control
        DWObject.RegisterEvent("OnPostLoad", function () {
            updatePageInfo();
        });

        // The event OnMouseClick fires when the mouse clicks on an image on Dynamic Web TWAIN viewer
        DWObject.RegisterEvent("OnMouseClick", function () {
            updatePageInfo();
        });

        // The event OnTopImageInTheViewChanged fires when the top image currently displayed in the viewer changes
        DWObject.RegisterEvent("OnTopImageInTheViewChanged", function (index) {
            DWObject.CurrentImageIndexInBuffer = index;
            updatePageInfo();
        });
    }
}

function OnSuccess() {
    console.log('successful');
}

function OnFailure(errorCode, errorString) {
    alert(errorString);
}

function LoadImage() {
    alert('Mostrar archivos');
    if (DWObject) {
        DWObject.IfShowFileDialog = true; // Open the system's file dialog to load image
        DWObject.LoadImageEx("", EnumDWT_ImageType.IT_ALL, OnSuccess, OnFailure); // Load images in all supported formats (.bmp, .jpg, .tif, .png, .pdf). OnSuccess or OnFailure will be called after the operation
    }
}

function AcquireImage() {
    alert('escanear');
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

function btnFirstImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = 0;
    updatePageInfo();
}

function btnPreImage_wheel() {
    if (DWObject.HowManyImagesInBuffer != 0)
        btnPreImage_onclick()
}

function btnNextImage_wheel() {
    if (DWObject.HowManyImagesInBuffer != 0)
        btnNextImage_onclick()
}

function btnPreImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    else if (DWObject.CurrentImageIndexInBuffer == 0) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = DWObject.CurrentImageIndexInBuffer - 1;
    updatePageInfo();
}
function btnNextImage_onclick() {
    if (!checkIfImagesInBuffer()) {
        return;
    }
    else if (DWObject.CurrentImageIndexInBuffer == DWObject.HowManyImagesInBuffer - 1) {
        return;
    }
    DWObject.CurrentImageIndexInBuffer = DWObject.CurrentImageIndexInBuffer + 1;
    updatePageInfo();
}

function updatePageInfo() {
    if (DWObject) {
        document.getElementById("DW_TotalImage").value = DWObject.HowManyImagesInBuffer;
        document.getElementById("DW_CurrentImage").value = DWObject.CurrentImageIndexInBuffer + 1;
    }
}

function source_onchange() {
    if (document.getElementById("divTwainType"))
        document.getElementById("divTwainType").style.display = "";
    if (document.getElementById("btnScan"))
        document.getElementById("btnScan").value = "Scan";

    if (document.getElementById("source"))
        DWObject.SelectSourceByIndex(document.getElementById("source").selectedIndex);
}