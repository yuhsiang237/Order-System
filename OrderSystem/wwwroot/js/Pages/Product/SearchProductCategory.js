(function ($) {
   /**
    *  Todo Bind UI Hadnler when the page ready.
    */
    $(document).ready(function () {
        // productTable
        $('#productTable').bind('click', function (e) {
            var t = $(e.target)
            if (t.attr('name') == "btn_updateModal") {
                $('#ProductCategoryUpdateModal').modal('show');
                UpdateModal.$data.Name = t.attr("data-Name")
                UpdateModal.$data.Id = t.attr("data-Id")
                UpdateModal.$data.Description = t.attr("data-Description")
                UpdateModal.$data.Errors = {}
            }
            if (t.attr('name') == "btn_deleteProduct") {
                if (confirm('確定要刪除'+ t.attr('data-Name') + '?')) {
                    deleteProductCategory(t.attr("data-Id"))
                }
            }
        })
        // list table control
        $('#btn-clearSearch').bind('click', function () {
            clearSearch()
        })
        $('#select_goToPage').bind('change', function () {
            goToPage()
        })
        $('#select_changePageSize').bind('change', function () {
            changePageSize()
        })
        // ./list table control
        $('#btn_productCategoryModal').bind("click", function () {
            // show modal
            $('#ProductCategoryCreateModal').modal('show');
        })


        var CreateModal = new Vue({
            el: '#ProductCategoryCreateModal',
            data: {
                Name: "",
                Description: "",
                Errors: {}
            },
            methods: {
                create() {
                    $.ajax({
                        type: 'POST',
                        context:this,
                        url: '/Product/CreateProductCategory',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            Name: this.Name,
                            Description: this.Description
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                alert('新增成功')
                                location.reload();
                            } else{
                                this.Errors = res.Error;
                            }
                        },
                        error: function () { alert('A error'); }
                    })
                }
            }
        })
        var UpdateModal = new Vue({
            el: '#ProductCategoryUpdateModal',
            data: {
                Id: "",
                Name: "",
                Description: "",
                Errors: {}
            },
            methods: {
                update() {
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/Product/UpdateProductCategory',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            Id: this.Id,
                            Name: this.Name,
                            Description: this.Description
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                alert('更新成功')
                                location.reload();
                            } else {
                                this.Errors = res.Error;
                            }
                        },
                        error: function () { alert('A error'); }
                    })
                }
            }
        })
    });


    function deleteProductCategory(Id) {
        $.ajax({
            type: 'POST',
            url: '/Product/DeleteProductCategory',
            contentType: 'application/x-www-form-urlencoded',
            headers: {
                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            data: {
                "Id":Id,
            },
            success: function (res) {
                if (res.IsSuccess) {
                    alert('刪除成功!')
                    location.reload();
                } 
            },
            error: function () { alert('A error'); }
        })
    }
    // list table
    function clearSearch() {
        $('#input_number').val('')
        $('#input_name').val('')
        $('#select_goToPage').val(1);
        $('#form_search').submit()
    }
    function goToPage() {
        $('#form_search').submit()
    }
    function changePageSize() {
        $('#form_search').submit()
    }
    // ./list table
})(jQuery)