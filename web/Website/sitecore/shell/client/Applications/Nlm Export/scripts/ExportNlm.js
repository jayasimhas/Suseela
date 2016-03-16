define(["sitecore"], function(_sc) {
    var ExportNlmDialog = _sc.Definitions.App.extend({
        initialized: function () {
        },

        close: function () {
            this.closeDialog(null);
        },

        submit: function () {
            var result = {
                delete: $('input[name=delete]')[0].checked,
                pubtype: $('input[name=pubtype]').val()
            };

            this.closeDialog(JSON.stringify(result));
        }
    });

    return ExportNlmDialog;
});