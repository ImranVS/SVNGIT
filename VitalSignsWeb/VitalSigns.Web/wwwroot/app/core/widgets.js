System.register(['./widgets/components/widget-container', './widgets/components/widget-controller'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    function exportStar_1(m) {
        var exports = {};
        for(var n in m) {
            if (n !== "default") exports[n] = m[n];
        }
        exports_1(exports);
    }
    return {
        setters:[
            function (widget_container_1_1) {
                exportStar_1(widget_container_1_1);
            },
            function (widget_controller_1_1) {
                exportStar_1(widget_controller_1_1);
            }],
        execute: function() {
        }
    }
});
//# sourceMappingURL=widgets.js.map