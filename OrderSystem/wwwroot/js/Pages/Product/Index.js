
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

$("#btn_productCreate").bind("click", function () {
    $.ajax({
        type: 'POST',
        url: '/Product/CreateProduct',
        contentType: 'application/x-www-form-urlencoded',
        headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: {
            "Name":$('#product_name').val(),
            "price": $('#product_price').val(),
            "number": $('#product_number').val(),
            "currentUnit": $('#product_currentUnit').val(),
            "description": $('#product_description').val()
        },
        success: function (res) {
            if (res.isSuccess) {
                $('#productCreateModal').modal('hide');
                alert('新增成功!')
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