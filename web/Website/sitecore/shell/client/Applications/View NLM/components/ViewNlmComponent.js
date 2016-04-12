//(function (speak) {
//    speak.component({
//        name: "ExportPrompt",
//        initialize: function (initial, app, el, sitecore) {
//            window.alert('hello from cshtml');
//        }
//    });
//})(Sitecore.Speak);


//define(["sitecore"], function (Sitecore) {
//    var model = Sitecore.Definitions.Models.ControlModel.extend({
//        initialize: function (options) {
//            this._super();
//            this.set("json", null);
//            alert('Inside Jsondatasource Init');
//        },
//        add: function (data) {

//            var json = this.get("json");
//            if (json === null)
//                json = new Array();

//            // this is done because array.push changes the array to an object which then do no work on the SPEAK listcontrol.
//            var newArray = new Array(json.length + 1);
//            for (var i = json.length - 1; i >= 0; i--)
//                newArray[i + 1] = json[i];
//            newArray[0] = data;
//            this.set("json", newArray);
//        }
//    });

//    var view = Sitecore.Definitions.Views.ControlView.extend({
//        initialize: function (options) {
//            this._super();
//            this.model.set("json", null);
//        }
//    });

//    Sitecore.Factories.createComponent("ExportPrompt", model, view, ".x-sitecore-jsondatasource");

//});