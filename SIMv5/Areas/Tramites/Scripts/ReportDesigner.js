(function (reportDesigner) {
    reportDesigner.CustomizeMenuActions.AddHandler(function (_, e) {
        var saveAction = e.Actions.filter(function (x) { return x.text === 'Save' })[0];
        saveAction.text = 'Save and Close';
    });
    reportDesigner.SaveCommandExecuted.AddHandler(function (_, e) {
        var result = JSON.parse(e.Result);
        window.location.href = result.redirectUrl;
    });
})(reportDesigner);