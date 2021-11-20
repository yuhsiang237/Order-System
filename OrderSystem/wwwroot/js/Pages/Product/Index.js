(function ($) {
   /**
    *  Todo Bind UI Hadnler when the page ready.
    */
    $(document).ready(function () {
        // productTable
        $('#productTable').bind('click', function (e) {
            var t = $(e.target)
            if (t.attr('name') == "btn_changeProductUnitModal") {
                changeProductUnitModal(t.attr("data-Id"), t.attr("data-Name"), t.attr("data-Number"), t.attr("data-CurrentUnit"))
            }
            if (t.attr('name') == "btn_updateProductModal") {
                updateProductModal(t.attr("data-Id"), t.attr("data-Name"), t.attr("data-Number"), t.attr("data-Price"), t.attr("data-CurrentUnit"), t.attr("data-Description"))
            }
            if (t.attr('name') == "btn_deleteProduct") {
                if (confirm('確定要刪除[' + t.attr('data-Number') + "]" + t.attr('data-Name') + '?')) {
                    deleteProduct(t.attr("data-Id"))

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

        $("#btn_changeProductUnit").bind("click", function () {

            $.ajax({
                type: 'POST',
                url: '/Product/UpdateProductUnit',
                contentType: 'application/x-www-form-urlencoded',
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                data: {
                    "Id": $('#uId').val(),
                    "CurrentUnit": $('#uNewUnit').val(),
                    "Description": $('#uDescription').val()
                },
                success: function (res) {
                    if (res.isSuccess) {
                        $('#btn_productUnitChangeModal').modal('hide');
                        alert('更新成功!')
                        location.reload();
                    } else {
                        if (res.error.CurrentUnit) {
                            $('#uErrorCurrentUnit').html(...res.error.CurrentUnit);
                        } else {
                            $('#uErrorCurrentUnit').html("");
                        }

                    }
                },
                error: function () { alert('A error'); }
            })
        });


        $('#btn_productModal').bind("click", function () {
            // clean
            $('#product_name').val(""),
                $('#product_price').val(""),
                $('#product_number').val(""),
                $('#product_currentUnit').val(""),
                $('#product_description').val("")
            // show modal
            $('#productCreateModal').modal('show');
        })

        $("#btn_productUpdate").bind("click", function () {
            $.ajax({
                type: 'POST',
                url: '/Product/UpdateProduct',
                contentType: 'application/x-www-form-urlencoded',
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                data: {
                    "Id": $('#update_product_Id').val(),
                    "Name": $('#update_product_name').val(),
                    "price": $('#update_product_price').val(),
                    "description": $('#update_product_description').val()
                },
                success: function (res) {
                    if (res.isSuccess) {
                        $('#productUpdateModal').modal('hide');
                        alert('更新成功!')
                        location.reload();
                    } else {
                        if (res.error.Name) {
                            $('#update_errorName').html(...res.error.Name);
                        } else {
                            $('#update_errorName').html("");
                        }
                        if (res.error.Price) {
                            $('#update_errorPrice').html(...res.error.Price);
                        } else {
                            $('#update_errorPrice').html("");
                        }
                        if (res.error.Number) {
                            $('#update_errorNumber').html(...res.error.Number);
                        } else {
                            $('#update_errorNumber').html("");
                        }
                    }
                },
                error: function () { alert('A error'); }
            })
        });

        $("#btn_productCreate").bind("click", function () {
            $.ajax({
                type: 'POST',
                url: '/Product/CreateProduct',
                contentType: 'application/x-www-form-urlencoded',
                headers: {
                    "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                data: {
                    "Name": $('#product_name').val(),
                    "price": $('#product_price').val(),
                    "number": $('#product_number').val(),
                    "currentUnit": $('#product_currentUnit').val(),
                    "description": $('#product_description').val()
                },
                success: function (res) {
                    if (res.isSuccess) {
                        $('#productCreateModal').modal('hide');
                        alert('新增成功!')
                        clearSearch()
                    } else {
                        if (res.error.Name) {
                            $('#errorName').html(...res.error.Name);
                        } else {
                            $('#errorName').html("");
                        }
                        if (res.error.Price) {
                            $('#errorPrice').html(...res.error.Price);
                        } else {
                            $('#errorPrice').html("");
                        }
                        if (res.error.Number) {
                            $('#errorNumber').html(...res.error.Number);
                        } else {
                            $('#errorNumber').html("");
                        }
                        if (res.error.CurrentUnit) {
                            $('#errorCurrentUnit').html(...res.error.CurrentUnit);
                        } else {
                            $('#errorCurrentUnit').html("");
                        }
                    }
                },
                error: function () { alert('A error'); }
            })
        });
    });
   
    function deleteProduct(Id) {
        $.ajax({
            type: 'POST',
            url: '/Product/DeleteProduct',
            contentType: 'application/x-www-form-urlencoded',
            headers: {
                "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            data: {
                "Id":Id,
            },
            success: function (res) {
                if (res.isSuccess) {
                    alert('刪除成功!')
                    location.reload();
                } 
            },
            error: function () { alert('A error'); }
        })
    }
    function updateProductModal(Id, Name, Number, Price,CurrentUnit,Description) {
        $('#update_product_Id').val(Id)
        $('#update_product_name').val(Name)
        $('#update_product_number').val(Number)
        $('#update_product_price').val(Price)
        $('#update_product_currentUnit').val(CurrentUnit)
        $('#update_product_description').val(Description)
        $('#productUpdateModal').modal('show')
    }
    function changeProductUnitModal(Id, Name, Number, CurrentUnit) {
        $('#uId').val(Id)
        $('#uNumber').val(Number)
        $('#uName').val(Name)
        $('#uCurrentUnit').val(CurrentUnit)
        $('#uNewUnit').val("")
        $('#uDescription').val("")
        $('#productUnitChangeModal').modal('show')
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