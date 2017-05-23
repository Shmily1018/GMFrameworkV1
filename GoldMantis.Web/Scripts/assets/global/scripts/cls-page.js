
/**
 * DESCRIPTION:          通用列表页脚本
 * AUTHOR:               CHEN JIN DONG
 * CREATE DATE:          2015年9月9日16:54:57
 */
var commonPage = new Object();

commonPage = {
    _export_to_excel_flag: false,
    _export_to_excel_index: 0,
    _table_enableHilight: true,

    _table_container_selector: ".common-table-container",
    _table_selector: ".common-table-container" + " .table",
    _table_header_selector: ".common-table-container" + " .table" + " .common-table-header",

    _table_data_order_name_selector: ".common-table-container" + " .table" + " .common-table-header" + " input[data-order-name='']",
    _table_data_order_direction_selector: ".common-table-container" + " .table" + " .common-table-header" + " input[data-order-direction='']",
    _table_data_order_th_selector: ".common-table-container" + " .table" + " .common-table-header" + " th[data-order-field]",


    _table_checkbox_all_selector: ".common-table-container" + " .table" + " .common-table-header" + " .common-table-checkbox-all",

    _table_checkbox_radio_selector: ".common-table-container" + " .table" + " tbody tr input[type='checkbox'], tbody tr input[type='radio']",
    _table_checkbox_selector: ".common-table-container" + " .table" + " tbody tr input[type='checkbox']",
    _table_radio_selector: ".common-table-container" + " .table" + " tbody tr input[type='radio']",

    /* 异步提交成功高亮的数据*/
    highlightData: null,

    /* 异步提交成功后执行的函数*/
    callbackSuccess: function () { },

    /* 异步提交出错后执行的函数*/
    callbackError: function () { },

    /* 异步提交前执行的函数*/
    callbackBefore: function () { },

    /* 对象初始化*/
    initialize: function () {
        this.initializeCommonSearch();
        this.initializeClear();
        this.highlightKeyword();
        this.initializeTable();

    },

    /* 初始化Table
    ----------------------------------------------------------*/
    initializeTable: function () {
        if (!/msie/.test(navigator.userAgent.toLowerCase()))
            $(document).css("height", "100%");


        this.initializeTableTip();

        /**
         * 单个checkbox的点击事件
         */
        this.setCheckBoxChange();

        /**
         * 单个radio的点击事件
         */
        this.setRadioChange();


        /**
        * 全部选中的checkbox的点击事件
        */
        this.setCheckBoxAllChange();

        this.fieldOrderInitialize();

        this.resizeTable();
    },


    /* 初始化显示和输入*/
    initializeInputDisplay: function () {

        /* 如果文本框为Readonly 则改变背景颜色*/
        //$("input[type='text'][readonly]").css("background", "#eee");

        /**
         * 设置只能输入小数
         */
        $(".decimal-input").bind("paste", function () { return false; });
        $(".decimal-input").forceDecimal();
        $(".decimal-input").bind("blur", function () {
            if ($.trim($(this).val()) == "") {
                return;
            }
            $(this).val($(this).val().replace(/,/gi, ''));

            var thisValue = $.trim($(this).val());
            if (thisValue.substring(thisValue.length - 1) == ".") {
                thisValue = ",";
            }
            if (isNaN(thisValue)) {
                $(this).val("");
                var docusDomID = document.activeElement.id;
                $(this).focus();
                $("#" + docusDomID).focus();
                $(this).blur();
            }

            $(this).val(commonPage.commafy(parseFloat($(this).val())));
        });

        /**
         * 设置只能输入整数
         */
        $(".numeric-input").bind("paste", function () { return false; });
        $(".numeric-input").forceNumeric();
        $(".numeric-input").bind("blur", function () {

            if ($(this).val() == "") {
                return;
            }
            else {
                //去除千分位
                $(this).val($(this).val().replace(/,/gi, ''));

                var val = parseInt($(this).val());
                if (val > parseInt($(this).attr("maxValue")) || val < parseInt($(this).attr("minValue"))) {
                    $(this).val("");
                    var docusDomID = document.activeElement.id;
                    $(this).focus();
                    $("#" + docusDomID).focus();
                    $(this).blur();
                }
            }
            if (isNaN($(this).val())) {
                $(this).val("");
                var docusDomID = document.activeElement.id;
                $(this).focus();
                $("#" + docusDomID).focus();
                $(this).blur();
            }
            $(this).val(commonPage.commafy(parseInt($(this).val())));
        });

        /**
         * 数字格式化
         */
        $('.decimal').each(
			function () {
			    $(this).val(window.formatNumber($(this).val(), 2));
			}
		);
        $('.numeric').each(
		    function () {
		        $(this).val(window.formatNumber($(this).val(), 0));
		    }
	    );

        /**
         * 日期选择，强制readonly
         */
        //$(".date-picker").datepicker({
        //    dateFormat: "yy-mm-dd",
        //    changeMonth: true,
        //    changeYear: true,
        //    showAnim: 'blind'
        //})
        // .bind("paste", function () { return false; })
        //.bind("mouseup", function () { $(this).focus(); $(this).select(); })
        //.forceReadOnly();

        $('.date-picker,.input-date').datepicker({
            orientation: "left",
            autoclose: true,
            clearBtn: true,
            todayBtn: true,
            todayHighlight: true
        });

        $('.datetime-picker,.input-datetime').datetimepicker({
            step: 30,
            format: "Y-m-d H:i"
        });


        /**
         * 日期格式化
         */
        $(".shortDateFormat").each(function (idx, elem) {
            if ($(elem).is(":input")) {
                if ("0001-01-01" != $.format.date($(elem).val(), "yyyy-MM-dd")) {
                    $(elem).val($.format.date($(elem).val(), "yyyy-MM-dd"));
                } else {
                    $(elem).val('');
                }

            }
            else {
                $(elem).text($.format.date($(elem).text(), "yyyy-MM-dd"));
            }
        });
        $(".longDateFormat").each(function (idx, elem) {
            if ($(elem).is(":input")) {
                if ("0001-01-01 12:00:00" != $.format.date($(elem).val(), "yyyy-MM-dd hh:mm:ss")) {
                    $(elem).val($.format.date($(elem).val(), "yyyy-MM-dd hh:mm:ss"));
                } else {
                    $(elem).val('');
                }
            } else {
                $(elem).text($.format.date($(elem).text(), "yyyy-MM-dd hh:mm:ss"));
            }
        });

    },


    /**
     * 初始化通用的查询条件
     * @returns {} 
     */
    initializeCommonSearch: function () {
        var commonSearchCondition = $('.common-search-condition');
        commonSearchCondition.addClass("form-control").addClass(" input-circle-left").attr('placeholder', '请输入查询条件...');
        commonSearchCondition.popover({
            'content': commonSearchCondition.attr('content'),
            'container': 'body',
            'animation': 'true',
            'placement': 'bottom',
            'trigger': 'hover'
        });
    },

    /**
     * 初始化TableTip
     * @returns {} 
     */
    initializeTableTip: function () {
        var tableTip = $('td[td-data-content]');
        tableTip.each(function (index, element) {
            $(element).popover({
                html: true,
                'content': $(element).attr('td-data-content'),
                'container': 'body',
                'animation': 'true',
                'placement': 'bottom',
                'trigger': 'hover'
            });
        });

    },

    /**
     * 清除查询条件
     * @returns {} 
     */
    initializeClear: function () {
        $(".button-clear").click(function () {
            $(".search-container input[type='text']").val("");
            $('.search-container select').each(function () {
                $(this).find('option:eq(0)').attr("selected", "selected");
            });
        });

        $(".button-search").click(function () {

            $("input[name='SearchEntity.PageIndex']").val("0");
            $(".common-search-condition").val("");
        });

        //需要初始化的数据
        $(".common-search-submit").click(function () {
            $("input[name='SearchEntity.PageIndex']").val("0");
            $(".button-clear").click();
            $('form').submit();
        });
    },


    /**
     * 初始化当前Form或者指定Form的异步提交.
     * @param {} option 
     * @returns {} 
     */
    ajaxGrid: function (option) {
        option = option || {};
        var _default = {
            target: '.replace-container',
            form: 'form',
            url: window.location.href,
            beforeSubmit: function () {
                Metronic.blockUI({
                    target: 'form',
                    boxed: true,
                    message: '数据加载中，请稍后...'
                });
                if (commonPage.callbackBefore) {
                    commonPage.callbackBefore.call();
                }


            },
            success: function (responseText, statusText) {
                Metronic.unblockUI('form');


                //修正IE9 TD错位问题、只需要对IE9进行处理
                commonPage.initializeTable();
                if (commonPage._table_enableHilight) {
                    commonPage.highlight(commonPage.highlightData);
                }

                if (commonPage.callbackSuccess) {
                    commonPage.callbackSuccess.call();
                }
            },
            error: function (responseText, statusText) {
                //会话过期，重定向到登录页
                if (responseText.status == "888") {
                    window.top.location.href = window.top.actionLogOn;
                }
                else {
                    window.top.bootstrapGM.alert("操作失败！");
                    if (commonPage.callbackError) {
                        commonPage.callbackError.call();
                    }
                }
                window.document.body.style.cursor = "default";
            },
            data: { "X-Requested-With": "XMLHttpRequest" }
        };
        $.extend(_default, option);
        _default.url = $(_default.form).attr("action") == undefined ? _default.url : $(_default.form).attr("action");
        $(_default.form).ajaxForm(_default);
    },

    /**
     * 异步提交当前Form,或可指定Form.
     * @param {} option 
     * @returns {} 
     */
    ajaxSubmit: function (option) {
        option = option || {};
        var _default = {
            form: 'form'
        };
        $.extend(_default, option);
        $(_default.form).submit();
    },


    /**
     * 条件查询显示与隐藏
     * @returns {} 
     */
    scan: function () {
        $(".search-container").css("display") == "none" ? $(".search-container").css("display", "block") : $(".search-container").css("display", "none");
        $(".search-container").css("display") == "block" ? $(".search-container").css("display", "block") : $(".search-container").css("display", "none");
        this.resizeTable();
    },


    /**
     * 获得分页控件中a 标签的当前页面url
     * @returns {} 
     */
    getCurrentUrl: function () {
        var url = "";
        var aUrl = $("a[paginate-current-url]").attr("paginate-current-url");
        if (aUrl == undefined || aUrl == null || aUrl == "")
            url = window.location.href;
        else
            url = aUrl;
        return url;
    },


    /**
     * 获得jquery table 对象
     * @returns {} 
     */
    getJTable: function () {
        return $(this._table_selector);
    },


    /**
     * 重新计算外侧frame高度
     * @returns {} 
     */
    resizeTable: function () {
        if (window.top.arrDialog.length > 0) {
            var currentDailog = window.top.arrDialog[window.top.arrDialog.length - 1];

            var bodyheight = $(currentDailog[0].contentWindow.document.body).height();
            $(currentDailog).height(bodyheight);

            var frameHeight = $(window.top).height() - 50 * 2;   //50:magin-top:50
            $(currentDailog).height(bodyheight);

            if ($(document).height() >= frameHeight) {
                currentDailog.parent().height(frameHeight - 63 - 30);  //30:Modal 标题高度，30：content padding
            }




        } else {
            $('iframe[class="active"]', parent.document).height($('body').height());
        }


    },


    /**
     * 获得第一个选中值
     * @returns {} 
     */
    getFirstSelectedValue: function () {
        var result = this.listSelectedValues();
        if (result.length > 0)
            return result[0];
        return null;
    },


    /**
     * 强制选择一行并返回选中的值
     * @returns {} 
     */
    forceSelectOne: function () {
        var result = this.listSelectedValues();
        if (result.length < 1) {
            //请至少选择一行记录！
            window.top.bootstrapGM.alert("请至少选择一行记录!");
            return null;
        }
        else if (result.length > 1) {
            //最多只能选择一行记录！
            window.top.bootstrapGM.alert("最多只能选择一行记录！");
            return null;
        }
        return result[0];
    },


    /**
     * 强列出所有选中值
     * @param {} keyName 
     * @returns {} 
     */
    listSelectedValues: function (keyName) {
        var result = [];
        if (keyName == null || keyName == undefined || keyName == "") {
            keyName = "ids";
        }
        $(this._table_checkbox_radio_selector).each(function () {
            if (this.checked) {
                result[result.length] = { name: keyName + '[' + result.length + ']', value: $(this).attr("value") };
            }
        });
        return result;
    },


    /**
     * 判断是否有选中数据
     * @returns {} 
     */
    isSelect: function () {
        var result = this.listSelectedValues();
        if (result.length < 1) {
            //请至少选择一行记录！
            window.top.bootstrapGM.alert("请至少选择一行记录!");
            return false;
        }
        return true;
    },


    /**
     * 全部选中的checkbox的点击事件
     * @returns {} 
     */
    setCheckBoxAllChange: function () {
        var tableInstance = this;
        $(this._table_checkbox_all_selector).click(function () {
            var status = this.checked;
            $(tableInstance._table_checkbox_selector).each(function () {
                if ($(this).attr("disabled"))
                    return;
                $(this).prop("checked", status);

                $(this).change();
                status ? $(this).parent().parent().parent("tr").addClass("common-table-tr-selected") : $(this).parent().parent("tr").removeClass("common-table-tr-selected");
            });
        });
    },


    /**
     * 单个checkbox的点击事件
     * @returns {} 
     */
    setCheckBoxChange: function () {
        $(this._table_checkbox_selector).change(function () {
            if ($(this).is(":checked")) {
                $(this).parent().parent().parent("tr").addClass("common-table-tr-selected");
            }
            else {
                $(this).parent().parent().parent("tr").removeClass("common-table-tr-selected");
            }
        });
    },



    /**
     * 单个Radio的点击事件
     * @returns {} 
     */
    setRadioChange: function () {
        $(this._table_radio_selector).change(function () {
            if ($(this).is(":checked")) {
                $(this).parent().parent().parent("tr").addClass("common-table-tr-selected");
                $(this).parent().parent().parent("tr").siblings().removeClass("common-table-tr-selected");
            }
            else {
                $(this).parent().parent().parent("tr").removeClass("common-table-tr-selected");
            }
        });
    },


    /**
     * 选择事件回调
     * @param {} callback 
     * @returns {} 
     */
    checkboxChangeEvent: function (callback) {
        $(this._table_checkbox_radio_selector).change(callback);
    },

    resize: function () {
        window.top.$('iframe[class="active"]', parent.document).height($('body').height());
    },

    fieldOrder: function (fieldName) {
        if ($(this._table_data_order_name_selector).val() == fieldName) {
            var orderDirection = $(this._table_data_order_direction_selector).val() == "0" ? "1" : "0";
            $(this._table_data_order_direction_selector).attr("value", orderDirection);
        }
        else {
            $(this._table_data_order_name_selector).attr("value", fieldName);
            $(this._table_data_order_direction_selector).attr("value", "0");
        }
        //commonPage.ajaxSubmit();
        $("form").submit();
    },
    /* Table排序初始化函数
    ----------------------------------------------------------*/
    fieldOrderInitialize: function () {
        var orderName = $(this._table_data_order_name_selector).val();
        var orderDirection = $(this._table_data_order_direction_selector).val();
        if (orderName == "" || orderDirection == "")
            return;
        $(this._table_data_order_th_selector).each(
		    function () {
		        var orderField = $(this).attr("data-order-field");
		        if (orderField != orderName)
		            return;
		        $(this).html($(this).html().replace("↑", "").replace("↓", ""));
		        orderDirection == "0" ? $(this).append("↑") : $(this).append("↓");
		    }
	    );
    },

    highlightKeyword: function () {
        var keyword = $.trim($(".common-search-condition").val());
        if (keyword == "" || keyword == "请输入查询条件...")
            return;
        $(this._table_container_selector).highlight(keyword);
    },

    highlight: function (highlightData) {
        if (highlightData == undefined || highlightData == null) {
            $(this._table_container_selector + " tbody tr").effect("highlight", {}, 1000);
            return;
        }
        $(this._table_checkbox_radio_selector).each(
			function () {
			    if (this.value != highlightData)
			        return;

			    $(this).parent().parent().parent("tr").effect("highlight", {}, 2000);
			}
		);
    },


    commafy: function (num) {

        if (isNaN(num)) {

            return "";

        }

        num = num + "";

        if (/^.*\..*$/.test(num)) {

            var pointIndex = num.lastIndexOf(".");

            var intPart = num.substring(0, pointIndex);

            var pointPart = num.substring(pointIndex + 1, num.length);

            intPart = intPart + "";

            var re = /(-?\d+)(\d{3})/

            while (re.test(intPart)) {

                intPart = intPart.replace(re, "$1,$2")

            }

            num = intPart + "." + pointPart;

        } else {

            num = num + "";

            var re = /(-?\d+)(\d{3})/

            while (re.test(num)) {

                num = num.replace(re, "$1,$2")

            }

        }

        return num;

    }

};


$(document).ready(function ($) {
    commonPage.initializeInputDisplay();
});



