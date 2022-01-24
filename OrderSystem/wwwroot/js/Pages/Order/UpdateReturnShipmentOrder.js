(function () {
    var vm = new Vue({
        el: '#Form',
        data: {
            ShipmentOrderId:null,
            IsSending: false, // prevent double send
            ReturnShipmentOrder: {},
            ReturnShipmentOrderDetails: [],
            Errors: {},
        },
        mounted: function () {
            this.ReturnShipmentOrderDetails = $Page.ReturnShipmentOrderDetails
            this.ReturnShipmentOrder = $Page.ReturnShipmentOrder
        },
        watch: {
            ShipmentOrderId: function (newVal, oldVal) {
                this.getShipmentOrderById(newVal)
            }
        },
        computed: {
            Total: function () {
              
            }
        },
        methods: {
            getAllShipmentOrderNumber() {
                $.ajax({
                    type: 'GET',
                    context: this,
                    url: '/Order/getAllShipmentOrderNumber',
                    contentType: 'application/x-www-form-urlencoded',
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    data: {},
                    success: function (res) {
                        if (res.IsSuccess) {
                            this.AllShipmentOrderNumber = res.Data
                        } else {
                            alert('訂單編號載入失敗')
                        }
                    },
                    complete: function (data) {
                    }
                })
            },
            getShipmentOrderById(Id) {
                $.ajax({
                    type: 'GET',
                    context: this,
                    url: '/Order/getShipmentOrderById?ShipmentOrderId='+Id,
                    contentType: 'application/x-www-form-urlencoded',
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    data: {},
                    success: function (res) {
                        if (res.IsSuccess) {
                            this.ReturnShipmentOrderDetails = res.Data.ShipmentOrderDetails
                            this.ReturnShipmentOrder = res.Data.ShipmentOrder
                        } else {
                            alert('訂單載入失敗')
                        }
                    },
                    complete: function (data) {
                    }
                })
            },
            createOrder() {
                if (this.IsSending === false) {
                    this.IsSending = true
                    $.ajax({
                        type: 'POST',
                        context: this,
                        url: '/Order/UpdateReturnShipmentOrder',
                        contentType: 'application/x-www-form-urlencoded',
                        headers: {
                            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                        },
                        data: {
                            ReturnShipmentOrder: this.ReturnShipmentOrder,
                            ReturnShipmentOrderDetails: this.ReturnShipmentOrderDetails.map(it => ({
                                Id: it.Id,
                                ShipmentOrderDetailId: it.ShipmentOrderDetailId,
                                Unit: it.Unit,
                                Remarks: it.Remarks,
                                })
                            )
                        },
                        success: function (res) {
                            if (res.IsSuccess) {
                                this.Errors = {};
                                alert('成功更新退貨單!')
                            } else {
                                this.Errors = res.Error;
                                alert('訂單建立失敗，請查看錯誤')
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
        $("#ReturnDate").datepicker().on("change", function (e) {
            vm.$data.ReturnShipmentOrder.ReturnDate = $(this).val();
        });
    })

})()
