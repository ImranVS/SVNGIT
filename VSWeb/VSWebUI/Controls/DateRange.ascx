<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRange.ascx.cs" Inherits="VSWebUI.Controls.DateRange" %>
<link href="../css/xcharts.min.css" rel="stylesheet" />
<link href="../css/datepickerstyle.css" rel="stylesheet" />
<link href="../css/daterangepicker.css" rel="stylesheet" />
<!-- Include bootstrap css -->
<link href="../css/bootstrap1.min.css" rel="stylesheet" />
<div id="contentChart">
    <form class="form-horizontal">
    <fieldset>
        <div class="input-prepend">
            <span class="add-on"><i class="icon-calendar"></i></span>
             <input type="text" name="range" id="range" ClientIDMode="Static"  onblur ="txtchanged()" autocomplete="off" runat="server" style="z-index:10"/>   <asp:TextBox ID="hfrange" runat="server" ClientIDMode="Static" style="visibility:hidden" Width="0px" Height="0px"></asp:TextBox>
             <input type="text" id="Text1" style="width:0px;height:0px;margin-left:-30px;margin-top:10px;z-index:9" />
             </div>     
    </fieldset>
     </form>
</div>
<script src="../js/jquery.min.js"></script>
<!-- The daterange picker bootstrap plugin "-->
<script src="../js/sugar.min.js"></script>
<script src="../js/daterangepicker.js"></script>
<!-- Our main script file -->
<script src="../js/script.js"></script>

<div class="daterangepicker dropdown-menu opensright" style="display: none; top: 75px;
    left: 415px; right: auto;">
    <div class="calendar right">
        <table class="table-condensed">
            <thead>
                <tr>
                    <th>
                    </th>
                    <th colspan="5" style="width: auto">
                        October 2014
                    </th>
                    <th class="next available">
                        <i class="icon-arrow-right"></i>
                    </th>
                </tr>
                <tr>
                    <th>
                        Su
                    </th>
                    <th>
                        Mo
                    </th>
                    <th>
                        Tu
                    </th>
                    <th>
                        We
                    </th>
                    <th>
                        Th
                    </th>
                    <th>
                        Fr
                    </th>
                    <th>
                        Sa
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="off disabled" title="r0c0">
                        29
                    </td>
                    <td class="off disabled" title="r0c1">
                        30
                    </td>
                    <td class="off disabled" title="r0c2">
                        1
                    </td>
                    <td class="off disabled" title="r0c3">
                        2
                    </td>
                    <td class="off disabled" title="r0c4">
                        3
                    </td>
                    <td class="off disabled" title="r0c5">
                        4
                    </td>
                    <td class="off disabled" title="r0c6">
                        5
                    </td>
                </tr>
                <tr>
                    <td class="off disabled" title="r1c0">
                        6
                    </td>
                    <td class="off disabled" title="r1c1">
                        7
                    </td>
                    <td class="available in-range start-date" title="r1c2">
                        8
                    </td>
                    <td class="available in-range" title="r1c3">
                        9
                    </td>
                    <td class="available in-range" title="r1c4">
                        10
                    </td>
                    <td class="available in-range" title="r1c5">
                        11
                    </td>
                    <td class="available in-range" title="r1c6">
                        12
                    </td>
                </tr>
                <tr>
                    <td class="available in-range" title="r2c0">
                        13
                    </td>
                    <td class="available active end-date" title="r2c1">
                        14
                    </td>
                    <td class="available" title="r2c2">
                        15
                    </td>
                    <td class="available" title="r2c3">
                        16
                    </td>
                    <td class="available" title="r2c4">
                        17
                    </td>
                    <td class="available" title="r2c5">
                        18
                    </td>
                    <td class="available" title="r2c6">
                        19
                    </td>
                </tr>
                <tr>
                    <td class="available" title="r3c0">
                        20
                    </td>
                    <td class="available" title="r3c1">
                        21
                    </td>
                    <td class="available" title="r3c2">
                        22
                    </td>
                    <td class="available" title="r3c3">
                        23
                    </td>
                    <td class="available" title="r3c4">
                        24
                    </td>
                    <td class="available" title="r3c5">
                        25
                    </td>
                    <td class="available" title="r3c6">
                        26
                    </td>
                </tr>
                <tr>
                    <td class="available" title="r4c0">
                        27
                    </td>
                    <td class="available" title="r4c1">
                        28
                    </td>
                    <td class="available" title="r4c2">
                        29
                    </td>
                    <td class="available" title="r4c3">
                        30
                    </td>
                    <td class="available" title="r4c4">
                        31
                    </td>
                    <td class="available off" title="r4c5">
                        1
                    </td>
                    <td class="available off" title="r4c6">
                        2
                    </td>
                </tr>
                <tr>
                    <td class="available off" title="r5c0">
                        3
                    </td>
                    <td class="available off" title="r5c1">
                        4
                    </td>
                    <td class="available off" title="r5c2">
                        5
                    </td>
                    <td class="available off" title="r5c3">
                        6
                    </td>
                    <td class="available off" title="r5c4">
                        7
                    </td>
                    <td class="available off" title="r5c5">
                        8
                    </td>
                    <td class="available off" title="r5c6">
                        9
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="calendar left">
        <table class="table-condensed">
            <thead>
                <tr>
                    <th class="prev available">
                        <i class="icon-arrow-left"></i>
                    </th>
                    <th colspan="5" style="width: auto">
                        October 2014
                    </th>
                    <th class="next available">
                        <i class="icon-arrow-right"></i>
                    </th>
                </tr>
                <tr>
                    <th>
                        Su
                    </th>
                    <th>
                        Mo
                    </th>
                    <th>
                        Tu
                    </th>
                    <th>
                        We
                    </th>
                    <th>
                        Th
                    </th>
                    <th>
                        Fr
                    </th>
                    <th>
                        Sa
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="available off" title="r0c0">
                        29
                    </td>
                    <td class="available off" title="r0c1">
                        30
                    </td>
                    <td class="available" title="r0c2">
                        1
                    </td>
                    <td class="available" title="r0c3">
                        2
                    </td>
                    <td class="available" title="r0c4">
                        3
                    </td>
                    <td class="available" title="r0c5">
                        4
                    </td>
                    <td class="available" title="r0c6">
                        5
                    </td>
                </tr>
                <tr>
                    <td class="available" title="r1c0">
                        6
                    </td>
                    <td class="available" title="r1c1">
                        7
                    </td>
                    <td class="available active start-date" title="r1c2">
                        8
                    </td>
                    <td class="available in-range" title="r1c3">
                        9
                    </td>
                    <td class="available in-range" title="r1c4">
                        10
                    </td>
                    <td class="available in-range" title="r1c5">
                        11
                    </td>
                    <td class="available in-range" title="r1c6">
                        12
                    </td>
                </tr>
                <tr>
                    <td class="available in-range" title="r2c0">
                        13
                    </td>
                    <td class="available in-range" title="r2c1">
                        14
                    </td>
                    <td class="available" title="r2c2">
                        15
                    </td>
                    <td class="available" title="r2c3">
                        16
                    </td>
                    <td class="available" title="r2c4">
                        17
                    </td>
                    <td class="available" title="r2c5">
                        18
                    </td>
                    <td class="available" title="r2c6">
                        19
                    </td>
                </tr>
                <tr>
                    <td class="available" title="r3c0">
                        20
                    </td>
                    <td class="available" title="r3c1">
                        21
                    </td>
                    <td class="available" title="r3c2">
                        22
                    </td>
                    <td class="available" title="r3c3">
                        23
                    </td>
                    <td class="available" title="r3c4">
                        24
                    </td>
                    <td class="available" title="r3c5">
                        25
                    </td>
                    <td class="available" title="r3c6">
                        26
                    </td>
                </tr>
                <tr>
                    <td class="available" title="r4c0">
                        27
                    </td>
                    <td class="available" title="r4c1">
                        28
                    </td>
                    <td class="available" title="r4c2">
                        29
                    </td>
                    <td class="available" title="r4c3">
                        30
                    </td>
                    <td class="available" title="r4c4">
                        31
                    </td>
                    <td class="available off" title="r4c5">
                        1
                    </td>
                    <td class="available off" title="r4c6">
                        2
                    </td>
                </tr>
                <tr>
                    <td class="available off" title="r5c0">
                        3
                    </td>
                    <td class="available off" title="r5c1">
                        4
                    </td>
                    <td class="available off" title="r5c2">
                        5
                    </td>
                    <td class="available off" title="r5c3">
                        6
                    </td>
                    <td class="available off" title="r5c4">
                        7
                    </td>
                    <td class="available off" title="r5c5">
                        8
                    </td>
                    <td class="available off" title="r5c6">
                        9
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="ranges">
        <ul>
            <li>Today1</li><li>Yesterday</li><li>Last 7 Days</li><li>Last 30 Days</li><li>Custom
                Range</li></ul>
        <div class="range_inputs">
            <div class="daterangepicker_start_input" style="float: left">
                Test<br />
                <br />
                <br />
                <br />
                <input class="input-mini" type="text" name="daterangepicker_start" value="" disabled="disabled"></div>
            <div class="daterangepicker_end_input" style="float: left; padding-left: 11px">
                <label for="daterangepicker_end">
                    To</label><br />
                <input class="input-mini" type="text" name="daterangepicker_end" value="" disabled="disabled"></div>
            <div>
                <button class="btn btn-small btn-success applyBtn">
                    Apply</button>&nbsp;
                <button class="btn btn-small clearBtn btn-success">
                    Clear</button>
            </div>
        </div>
    </div>
</div>
<div class="ex-tooltip">
</div>
<script type="text/javascript">
//Mukund: 19Nov14: TO PRESERVE VALUES WHEN PAGE RELOADS
    var startDate = "";var endDate = "";
    var oldval = document.getElementById("hfrange");
    var newval = document.getElementById("range");
    if (oldval.value != "") {
//        alert(oldval.value);
        newval.value = oldval.value;
        startDate = Date.create(newval.value.substring(0, newval.value.indexOf("-") - 1)); //  Date.create().addDays(-6), // 7 days ago
        endDate = Date.create(newval.value.substring(newval.value.indexOf("-") + 2)); //  Date.create();
    }
    else {
        startDate = Date.create().addDays(-6), // 7 days ago
		endDate = Date.create();
        oldval.value = startDate.format('{MM}/{dd}/{yyyy}') + ' - ' + endDate.format('{MM}/{dd}/{yyyy}');     
       
    }
            function txtchanged() {
                var oldval1 = document.getElementById("hfrange");
                var newval1 = document.getElementById("range");
                oldval1.value = newval1.value;

            }
//            function Daterange() {
//                this._defaults = {
//                    onClose: null
//                };
//            }
</script>
