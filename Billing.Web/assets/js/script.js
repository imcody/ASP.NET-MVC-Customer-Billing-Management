var Invoice = function () {

	// Handles Bootstrap Tooltips.
    var handleTooltips = function() {
        $('.tooltips').tooltip();
    };
    var WindowResize = function(){
    };
    var myDate = new Date();
    var date = myDate.getFullYear() + '/' + ('0' + myDate.getMonth()).slice(-2) + '/' + ('0' + myDate.getDate()).slice(-2);
    var calendar = function () {
        $(".calendar").datepicker({
            dateFormat: 'yyyy-mm-dd',
            changeMonth: true,
            changeYear: true,
            minDate: '',
            maxDate: ''
        });
    };

    //Site events triggering
    //With Buttons and links
    var SiteInit = function () {
        $(".calendar").datepicker({
            changeMonth: true,
            changeYear: true,
            minDate: '',
            maxDate: ''
        });
        $(".calendarFrom, .calendarTo").datepicker({
            changeMonth: true,
            changeYear: true,
            minDate: '',
            maxDate: ''
        });
        $('.trigger-calendar-from').click(function () {
            $(this).closest('.input-group').find('.calendar').focus();
        });
        $('.btnAdvance').click(function () {
            if($(this).hasClass('active')) {
                $('.advance-field').hide();
                $(this).removeClass('active');
                $(this).text("Advance Search");
            }
            else {
                $('.advance-field').show();
                $(this).addClass('active');
                $(this).text("Hide Advance");
            }
        });
        $(document).on('keyup', '.inv-amount', function () {
            var amount = $(this).val();
            var tax = $(this).closest('tr').find('.inv-tax').val();
            if (tax != undefined) {
                var fare = amount - tax;
            }
            else {
                var fare = amount;
            }
            $(this).closest('tr').find('.inv-fare').val(fare);
            var total = 0;
            $('.inv-row').each(function () {
                var amount = parseInt($(this).find('td > input.inv-amount').val());
                if (amount != undefined && amount > 0) {
                    total = total + amount;
                    $('.inv-total').text(total);
                    $('.inv-grandtotal').text(total);
                    $('#GrandTotal').val(total);
                }
            });
        });
        $(document).on('keyup', '.inv-tax', function () {
            var tax = $(this).val();
            var amount = $(this).closest('tr').find('.inv-amount').val();
            if (amount != undefined) {
                var fare = amount - tax;
            }
            else {
                var fare = amount;
            }
            $(this).closest('tr').find('.inv-fare').val(fare);
        });
        $(document).on('keyup', '.inv-extra', function () {
            var extra = parseInt($(this).val());
            var total = parseInt($('.inv-total').text());
            if (extra > 0) {
                var gtotal = total + extra;
                $('.inv-grandtotal').text(gtotal);
            }
            else {
                $('.inv-grandtotal').text(total);
            }
        });
        $(document).on('keyup', '.inv-vnetfare', function () {
            debugger;
            var amount = parseFloat($(this).closest('tr').find('.inv-fiamount').val());
            var tax = parseFloat($(this).closest('tr').find('.inv-fitax').val());
            var apc = parseFloat($(this).closest('tr').find('.inv-fiapc').val());
            var vnetfare = parseFloat($(this).val());
            var vcharge = parseFloat($(this).closest('tr').find('.inv-vcharge').val());
            var proft = amount - (tax + apc + vnetfare + vcharge);
            if (proft != NaN) {
                $(this).closest('tr').find('.inv-profit').val(proft);
            }
            else {
                $(this).closest('tr').find('.inv-profit').val(0);
            }
        });
        $(document).on('keyup', '.inv-vcharge', function () {
            debugger;
            var amount = parseFloat($(this).closest('tr').find('.inv-fiamount').val());
            var tax = parseFloat($(this).closest('tr').find('.inv-fitax').val());
            var apc = parseFloat($(this).closest('tr').find('.inv-fiapc').val());
            var vnetfare = parseFloat($(this).closest('tr').find('.inv-vnetfare').val());
            var vcharge = parseFloat($(this).val());
            var proft = amount - (tax + apc + vnetfare + vcharge);
            if (proft != NaN) {
                $(this).closest('tr').find('.inv-profit').val(proft);
            }
            else {
                $(this).closest('tr').find('.inv-profit').val(0);
            }
        });
        $(document).on('keyup', '.inv-fiamount', function () {
            debugger;
            var amount = parseFloat($(this).closest('tr').find('.inv-fiamount').val());
            var tax = parseFloat($(this).closest('tr').find('.inv-fitax').val());
            var cnetfare = parseFloat(amount - tax);
            if (cnetfare != NaN) {
                $(this).closest('tr').find('.inv-cnetfare').val(cnetfare);
            }
            else {
                $(this).closest('tr').find('.inv-cnetfare').val(0);
            }
        });
        $(document).on('keyup', '.inv-fitax', function () {
            debugger;
            var amount = parseFloat($(this).closest('tr').find('.inv-fiamount').val());
            var tax = parseFloat($(this).closest('tr').find('.inv-fitax').val());
            var cnetfare = parseFloat(amount - tax);
            if (cnetfare != NaN) {
                $(this).closest('tr').find('.inv-cnetfare').val(cnetfare);
            }
            else {
                $(this).closest('tr').find('.inv-cnetfare').val(0);
            }
        });
        $('.sidebar-collapse').click(function () {
            if ($(this).closest('.filter-section').hasClass('collapsed')) {
                $(this).closest('.filter-section').removeClass('collapsed');
                $(this).closest('.filter-section').find('.filter-body').slideDown();
            }
            else {
                $(this).closest('.filter-section').addClass('collapsed');
                $(this).closest('.filter-section').find('.filter-body').slideUp();
            }
        });
        $('.createInvoice').click(function () {
            Pageloader("show");
            $('#gdsModal').modal();
            $('#gdsModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getVendorList();
            Pageloader("hide");
        });
        $('#CancelInvoice').click(function () {
            var r = confirm("Are you sure you want to cancel the Invoice?");
            if (r) {
                return true;
            } else {
                return false;
            }
        });
        $('.updateAgent').click(function () {
            Pageloader("show");
            $('#agentModal').modal();
            $('#agentModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getAgentListUI();
            Pageloader("hide");
        });
        $('.updateAgent-oi').click(function () {
            var PaidByAgent = parseInt($('input#PaymentStarted').val());
            if (PaidByAgent > 0) {
                alert('You cant change the Agent of this Invoice now.');
                return false;
            }
            Pageloader("show");
            $('#agentModal').modal();
            $('#agentModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getAgentListUI();
            Pageloader("hide");
        });
        $('.updateVendor').click(function () {
            Pageloader("show");
            $('#vendorModal').modal();
            $('#vendorModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getVendorList();
            Pageloader("hide");
        });
        $('.updateVendor-oi').click(function () {
            var PaidByAgent = parseInt($('input#PaymentStarted').val());
            if (PaidByAgent > 0) {
                alert('You cant change the Vendor of this Invoice now.');
                return false;
            }
            Pageloader("show");
            $('#vendorModal').modal();
            $('#vendorModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getVendorList();
            Pageloader("hide");
        });
        $('.updateInvoice').click(function () {
            Pageloader("show");
            $('#invoiceModal').modal();            
            $('#invoiceModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            Pageloader("hide");            
        });
        $('.updateInvoice-oi').click(function () {
            $('span#updateInvoiceModalInvNo').html($('input#InvoiceId').val());
            Pageloader("show");
            $('#invoiceModal').modal();
            $('#invoiceModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getOtherTypeListUI();
            Pageloader("hide");
        });
        $('.transaction').click(function () {
            Pageloader("show");
            $('#transactionModal').modal();
            $('#transactionModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>');
            //$('div#tmInvNo').val($('input#InvoiceId').val())
            Pageloader("hide");
            $('#transactionModal').find('#tmInvNo').html($('input#FInvoiceId').val());
            $('#transactionModal').find('#tmAmount').html($('input#TotalAmount').val());
            $('#transactionModal').find('#tmExtra').html($('input#ExtraCharge').val());
            $('#transactionModal').find('#tmRefunded').html($('input#AmountRefunded').val());
            $('#transactionModal').find('#tmRecieved').html($('input#AmountReceived').val());
            $('#transactionModal').find('#AmountDue').html($('input#AmountDue').val());
        });
        $('.agentTransaction-oi').click(function () {
            Pageloader("show");
            $('#transactionModal').modal();
            $('#transactionModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>');
            Pageloader("hide");
            $('#transactionModal').find('#tmInvNo').html($('input#FInvoiceId').val());
            $('#transactionModal').find('#tmAmount').html($('input#TotalAmount').val());
            $('#transactionModal').find('#tmRecieved').html($('input#PaidAmount').val());
            $('#transactionModal').find('#tmAmntDue').html($('input#DueAmount').val());
        });
        $('.addRemarks').click(function () {
            Pageloader("show");
            $('#remarksModal').modal();
            $('#remarksModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            Pageloader("hide");
        });
        $('.addRemarks-oi').click(function () {
            Pageloader("show");
            $('#remarksModal').modal();
            $('#remarksModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            Pageloader("hide");
        });
        $('.AgentType').change(function () {
            Invoice.getAgentList();
        });
        $('.changeUser').click(function () {
            Pageloader("show");
            $('p#CurrentUserHtml').html($('input#CurrentUserName').val())
            $('#userModal').modal();
            $('#userModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getUserListUI();
            Pageloader("hide");
        });
        $('.changeUser-oi').click(function () {
            Pageloader("show");
            $('p#CurrentUserHtml').html($('input#CurrentUserName').val())
            $('#userModal').modal();
            $('#userModal').find('.modal-footer .loaderModal').append('<div class="modalLoader" style="display:none"></div>')
            getUserListUI();
            Pageloader("hide");
        });
    };
    //End of site button events and triggering
    var FillUpTicketPrice = function () {
        var amount = $('input#Amount1').val();
        var tax = $('input#Tax1').val();
        var apc = $('input#Apc1').val();
        var cnetfare = $('input#CNetFare1').val();
        var vnetfare = $('input#VNetFare1').val();
        var vCharge = $('input#VendorCharge1').val();
        var profit = $('input#Profit1').val();
        $('.inv-fiamount').val(amount);
        $('.inv-fitax').val(tax);
        $('.inv-fiapc').val(apc);
        $('.inv-cnetfare').val(cnetfare);
        $('.inv-vnetfare').val(vnetfare);
        $('.inv-vcharge').val(vCharge);
        $('.inv-profit').val(profit);
    };
    var FillUpTicketSegments = function () {
        var flight = $('input#FlightNo1').val();
        var clas = $('input#SegmentClass1').val();
        var ddate = $('input#DepartureDate1').val();
        var dfrom = $('input#DepartureFrom1').val();
        var depto = $('input#DepartureTo1').val();
        var arrtm = $('input#ArrivalTime1').val();
        var deptm = $('input#DepartureTime1').val();
        var sstat = $('input#SegmentStatus1').val();
        var fdate = $('input#FlightDate1').val();
        $('.inv-flightno').val(flight);
        $('.inv-segmentclass').val(clas);
        $('.inv-departuredate').val(ddate);
        $('.inv-departurefrom').val(dfrom);
        $('.inv-departureto').val(depto);
        $('.inv-arrivaltime').val(arrtm);
        $('.inv-departuretime').val(deptm);
        $('.inv-segmentstatus').val(sstat);
        $('.inv-flightdate').val(fdate);
    };
    var GetAgentQuickContact = function (agent) {
        $.get("/Home/GetQuickAgentContact", { AgentId: agent }, function (res) {
            $("#qATelephone").html(res.Telephone);
            $("#qAMobile").html(res.Mobile);
            $("#qAFaxNo").html(res.FaxNo);
            $("#qAAddress").html(res.Address);
            $("#qAPostcode").html(res.PostCode);
            $("#qAEmail").html(res.Email);
            $("#qARemarks").html(res.Remarks);
        });
    };

    var Notify = function (type, message, container, autohide) {
        // Notify Type 1 === Success
        // Notify Type 2 === Error
        // Notify Type 3 === Warning
        // Notify Type 4 === Info
        if (type == 1) {
            $('.' + container).html('');
            $('.' + container).append('<div class="alert alert-success alert-dismissible" style="display:none"><a href="#" class="close" title="close"></a><span class="message">' + message + '</span></div>');
            $('.' + container).find('.alert').slideDown();
            if (autohide != undefined) {
                setTimeout(function () {
                    $('.' + container).find('.alert').slideUp();
                }, 2000);
            }
        }
        else if (type == 2) {
            $('.' + container).html('');
            $('.' + container).append('<div class="alert alert-danger alert-dismissible" style="display:none"><a href="#" class="close" title="close"></a><span class="message">' + message + '</span></div>');
            $('.' + container).find('.alert').slideDown();
            if (autohide != undefined) {
                setTimeout(function () {
                    $('.' + container).find('.alert').slideUp();
                }, 2000);
            }
        }
        else if (type == 3) {
            $('.' + container).html('');
            $('.' + container).append('<div class="alert alert-warning alert-dismissible" style="display:none"><a href="#" class="close" title="close"></a><span class="message">' + message + '</span></div>');
            $('.' + container).find('.alert').slideDown();
            if (autohide != undefined) {
                setTimeout(function () {
                    $('.' + container).find('.alert').slideUp();
                }, 2000);
            }
        }
        else if (type == 4) {
            $('.' + container).html('');
            $('.' + container).append('<div class="alert alert-info alert-dismissible" style="display:none"><a href="#" class="close" title="close"></a><span class="message">' + message + '</span></div>');
            $('.' + container).find('.alert').slideDown();
            if (autohide != undefined) {
                setTimeout(function () {
                    $('.' + container).find('.alert').slideUp();
                }, 2000);
            }
        }
        $(document).on('click', '.alert .close', function () {
            $(this).closest('.alert').slideUp();
        });
        $('body').on('click', '.segment-modal-button', function (e) {
            e.preventDefault();
            $.get($(this).data("targeturl"), function (data) {
                $("#modal-body").html(data);
            });
        });
    };
    var getAgentList = function () {
        //debugger;
        ModalLoader("show");
        var ProfileType = $("select#ProfileType").val();
        if (ProfileType == 1) {
            $("#CustomerType").css("display", "none");
            $("#NewCustomerSection").css("display", "none");
            $("#CustomerList").css("display", "none");
            $.ajax({
                url: '/Invoice/GetAgentList',
                type: 'POST',
                datatype: 'application/json',
                contentType: 'application/json',
                data: JSON.stringify({ type: 1 }),
                success: function (result) {
                    $("#AgentList").css("display", "block");
                    $("#AgentId").html("");
                    $.each($.parseJSON(result), function (i, city) {
                        $("#AgentId").append($('<option></option>').val(city.Id).html(city.Name))
                    })
                    ModalLoader("hide");
                },
                error: function (xhr, status, error) {
                    $("#AgentList").css("display", "none");
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                    ModalLoader("hide");
                },
            });
        }
        else if (ProfileType == 2) {
            $("#AgentList").css("display", "none");
            $("#CustomerList").css("display", "none");
            $("#NewCustomerSection").css("display", "none");
            $("#CustomerType").css("display", "block");
            $("#CusType").change(function () {
                var CusType = $("#CusType").val();
                if (CusType == 1) {
                    ModalLoader("hide");
                    $("#AgentId2").css("display", "none");
                    $("#NewCustomerSection").css("display", "block");
                }
                else if (CusType == 2) {
                    debugger;
                    ModalLoader("hide");
                    $("#NewCustomerSection").css("display", "none");
                    $.ajax({
                        url: '/Invoice/GetAgentList',
                        type: 'POST',
                        datatype: 'application/json',
                        contentType: 'application/json',
                        data: JSON.stringify({ type: 2 }),
                        success: function (result) {
                            $("#CustomerList").css("display", "block");
                            $("#AgentId2").css("display", "block");
                            $("#AgentId2").html("");
                            //$("#AgentId").append($('<option></option>').val("").html("Select Agent"));
                            $.each($.parseJSON(result), function (i, city) {
                                $("#AgentId2").append($('<option></option>').val(city.Id).html(city.Name))
                            });
                            ModalLoader("hide");
                        },
                        error: function (xhr, status, error) {
                            $("#CustomerList").css("display", "none");
                            var err = eval("(" + xhr.responseText + ")");
                            alert(err.Message);
                            ModalLoader("hide");
                        },
                    });
                }
            });
        }
    };
    //agentModal update Invoice
    var getAgentListUI = function () {
        ModalLoader("show");
        var ProfileType = $("input#ProfileType").val();
        if (ProfileType == 1) {
            $.ajax({
                url: '/Invoice/GetAgentList',
                type: 'POST',
                datatype: 'application/json',
                contentType: 'application/json',
                data: JSON.stringify({ type: 1 }),
                success: function (result) {
                    $("select#AgentId").html("");
                    $.each($.parseJSON(result), function (i, city) {
                        $("select#AgentId").append($('<option></option>').val(city.Id).html(city.Name))
                    })
                    ModalLoader("hide");
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                    ModalLoader("hide");
                },
            });
        }
        else if (ProfileType == 2) {
            $.ajax({
                url: '/Invoice/GetAgentList',
                type: 'POST',
                datatype: 'application/json',
                contentType: 'application/json',
                data: JSON.stringify({ type: 2 }),
                success: function (result) {
                    $("select#AgentId").html("");
                    $.each($.parseJSON(result), function (i, city) {
                        $("select#AgentId").append($('<option></option>').val(city.Id).html(city.Name))
                    });
                    ModalLoader("show");
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                    ModalLoader("show");
                },
            });
        }
    };
    var getOtherTypeListUI = function () {
        ModalLoader("show");
        $.ajax({
            url: '/OtherInvoice/GetOtherInvoiceTypeList',
            type: 'POST',
            datatype: 'application/json',
            contentType: 'application/json',
            success: function (result) {
                $("select#OtherInvoiceTypeId").html("");
                $.each($.parseJSON(result), function (i, city) {
                    $("select#OtherInvoiceTypeId").append($('<option></option>').val(city.Id).html(city.InvoiceType))
                })
                ModalLoader("hide");
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                ModalLoader("hide");
            },
        });
    };
    var Pageloader = function (state) {
        if (state == "show") {
            $('body').find('.loader-container').show();
        }
        else{
            $('body').find('.loader-container').hide();
        }
    };
    var ModalLoader = function (state) {
        if (state == "show") {
            $('.modal').find('.modal-footer .modalLoader').show();
        }
        else {
            $('.modal').find('.modal-footer .modalLoader').hide();
        }
    };
    var getVendorList = function () {
        ModalLoader("show");
        $.ajax({
            url: '/Invoice/GetVendorList',
            type: 'POST',
            datatype: 'application/json',
            contentType: 'application/json',
            success: function (result) {
                $("select#VendorId").html("");
                //$("#VendorId").append($('<option></option>').val("").html("Select Vendor"));
                $.each($.parseJSON(result), function (i, vendor) {
                    $("select#VendorId").append($('<option></option>').val(vendor.Id).html(vendor.Name));
                })
                ModalLoader("hide");
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                ModalLoader("hide");
            },
        });
    };
    var ActiveFocus = function () {
        var url = window.location;
        $('ul.nav a[href="' + url + '"]').parent().addClass('active');
        $('ul.nav a').filter(function () {
            return this.href == url;
        }).parent().addClass('active');
    };
    var getUserListUI = function () {
        ModalLoader("show");
        $.ajax({
            url: '/Account/GetUserList',
            type: 'POST',
            datatype: 'application/json',
            contentType: 'application/json',
            data: JSON.stringify({ type: 1 }),
            success: function (result) {
                $("select#ApplicationUserId").html("");
                $.each($.parseJSON(result), function (i, city) {
                    $("select#ApplicationUserId").append($('<option></option>').val(city.Id).html(city.PersonName))
                })
                ModalLoader("hide");
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
                ModalLoader("hide");
            },
        });
    };
    return {
        getAgentList: getAgentList,
        getVendorList: getVendorList,
        ActiveFocus: ActiveFocus,
        FillUpTicketPrice: FillUpTicketPrice,
        FillUpTicketSegments: FillUpTicketSegments,
        GetAgentQuickContact: GetAgentQuickContact,
        Pageloader:Pageloader,
        Notify: Notify,
        ModalLoader: ModalLoader,
        calendar:calendar,
        init: function() {
            handleTooltips();
            //iCheck();
            WindowResize();
            SiteInit();
            Notify();
        }
    };
}();