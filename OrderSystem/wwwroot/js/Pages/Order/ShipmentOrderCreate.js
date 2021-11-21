(function () {
    var vm = new Vue({
        el: '#CreateForm',
        data: {
            Number: "",
            Type: "",
            DeliveryDate: "",
            FinishDate: "",
            Total: "",
            Remarks: "",
            Address: "",
            OrderDetails: [
                {
                    Number: "C-1",
                    Unit: 5,
                    Name: "商品1",
                    Remarks: "備註"
                },
                {
                    Number: "C-1",
                    Unit: 5,
                    Name: "商品1",
                    Remarks: "備註"
                }
            ]
        },
        methods: {
            deleteRow(index) {
                this.OrderDetails.splice(index, 1);
            },
            createOrder() {
                $.ajax({
                    type: 'POST',
                    url: '/Order/ShipmentOrderCreate',
                    contentType: 'application/x-www-form-urlencoded',
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    data: {
                        Number:this.Name,
                        Type: this.Type,
                        DeliveryDate: this.DeliveryDate,
                        FinishDate: this.FinishDate,
                        Total: this.Total,
                        Remarks: this.Remarks,
                        Address: this.Address,
                    },
                    success: function (res) {
                        alert('送出!')
                    },
                    error: function () { alert('A error'); }
                })
            }
        }
    })
    $(document).ready(function () {
        $("#DeliveryDate").datepicker().on("change", function (e) {
            vm.$data.DeliveryDate = $(this).val();
        });
        $("#FinishDate").datepicker().on("change", function (e) {
            vm.$data.FinishDate = $(this).val();
        });
    })

})()
