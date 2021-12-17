//jstree 
$(function () {
    $('#jstree1').jstree({'plugins':["wholerow","checkbox"], 'core' : {
        'data' : [
            {
                "text" : "系統管理", 
                "children" : [
                    { "text" : "公司資料管理", "state" : { "selected" : true } },
                    { "text" : "系統參數設定" },
                    { "text" : "系統選單設定",
                      "children" : [
                          { "text" : "第三層選項-1" },
                          { "text" : "第三層選項-2" }
                      ]                        
                    },
                    { "text" : "角色權限設定" }
                ]
            },
            {
                "text" : "基本資料",
                "children" : [
                    { "text" : "公司資料管理" },
                    { "text" : "系統參數設定" },
                    { "text" : "系統選單設定" },
                    { "text" : "角色權限設定" }
                ]
            }
        ]
    }});
});	
