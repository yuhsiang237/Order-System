(function () {
    var vm = new Vue({
        el: '#Form',
        data: {
            OrderId:"",
            IsSending: false, // prevent double send
            Number: "",
            Type: "",
            DeliveryDate: "",
            FinishDate: "",
            Remarks: "",
            Total:"",
            Address: "",
            SignName: "",
            Errors: {},
            OrderDetails: []
        },
        mounted: function () {
            if ($Page.Order) {
                this.OrderId = $Page.Order.Id
                this.Number = $Page.Order.Number
                this.DeliveryDate = $Page.Order.DeliveryDate
                this.FinishDate = $Page.Order.FinishDate
                this.Address = $Page.Order.Address
                this.Remarks = $Page.Order.Remarks
                this.SignName = $Page.Order.SignName
                this.Total = $Page.Order.Total
            }
            if ($Page.OrderDetails) {
                this.OrderDetails = $Page.OrderDetails
            }
        },
        methods: {
            createRow() {
                this.OrderDetails.push({});
            },
            deleteRow(index) {
                this.OrderDetails.splice(index, 1);
            },
            updateOrder() {
                if (this.IsSending === false) {
                    this.IsSending = true
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/Order/UpdateShipmentOrder',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            ShipmentOrder: {
                                Id: this.OrderId,
                                Number: this.Number,
                                Type: this.Type,
                                DeliveryDate: this.DeliveryDate,
                                FinishDate: this.FinishDate,
                                Total: this.Total,
                                Remarks: this.Remarks,
                                Address: this.Address,
                                SignName: this.SignName
                            }
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                this.Errors = {};
                                alert('成功更新訂單!')
                            } else {
                                this.Errors = res.Error;
                                alert('訂單更新失敗，請查看錯誤')
                            }
                        },
                        complete: function (data) {
                            this.IsSending = false;
                        }
                    })
                }
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
