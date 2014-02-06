// Add validation check to hidden fields with the .validate class (since jQuery validator does not validate hidden fields)
$.validator.setDefaults({ ignore: ':hidden:not(".validate")' });        // $.validator.setDefaults({ ignore: $(":hidden").not('.validate') }); also works

// Add method to check both first and last name
/*$.validator.addMethod("bothNames", function (value, element, params) {
    var firstName = $('input[id="' + params[0] + '"]').val(),
        lastName  = $('input[id="' + params[1] + '"]').val();
    if (firstName === "" && lastName === "") {
        return false;
    }
    else {
        return true;
    }
}, "Both First and Last Name are required.");*/


$(document).ready(
    function () {

        // Bind to the change event (which is triggered in Appraisers.js) and revalidate the hidden.validate fields since jQuery validator does not automatically validate hidden fields.
        $('.validate').on("change", function () {
            $("#OrderAppraisal").validate().element(".validate");
        });

        // Add validation rules for the order form
        $("#Appraisal_ClientPerson_FirstName").rules("add", { required: true, messages: { required: "First Name is required." } });
        $("#Appraisal_ClientPerson_LastName").rules("add", { required: true, messages: { required: "Last Name is required." } });
        //$("#ClientPersonFullName").rules("add", { bothNames: ["Appraisal_ClientPerson_FirstName", "Appraisal_ClientPerson_LastName", "ClientPersonFullName"] });
        $("#Appraisal_ClientPerson_Phone").rules("add", { required: true, messages: { required: "Phone Number is required." } });
        $("#Appraisal_ClientPerson_Email").rules("add", { required: true, messages: { required: "Email is required." } });
        $("#Appraisal_ClientPerson_Email").rules("add", { email: true });
        $("#Appraisal_PropertyAddress_Address1").rules("add", { required: true, messages: { required: "Address is required." } });
        $("#Appraisal_PropertyAddress_City").rules("add", { required: true, messages: { required: "City is required." } });

        // Add phone mask
        $(".phone-number").mask("(999) 999-9999");


        $("#CancelForm").on("click", function (e) {
            $("span.field-validation-error").html("");
            $("span.field-validation-valid").html("");
            $(".blank-first-child").trigger("change");   // triggers change to update display color
        });

        // Formatting/display to show "placeholder" coloring for DropDownBoxes with an empty first option
        $(".blank-first-child").on("change", function () {
            if ($("option:selected", this).index() === 0) {
                $(this).addClass("select-placeholder");
            }
            else {
                $(this).removeClass("select-placeholder");
            }
        });

        // Tooltips
        $("[data-toggle='tooltip']").tooltip({
            'placement': 'right'
        });

        $('input:radio[id="AreYouClient"]').change(
            function () {
                if ($(this).is(':checked') && $(this).val() == 'No') {
                    $("#OrderAppraisal #ClientInfo").show("fadeIn", "swing", "slow");

                    // Add validation rules
                    $("#Appraisal_Client2Person_FirstName").rules("add", { required: true, messages: { required: "First Name is required." } });
                    $("#Appraisal_Client2Person_LastName").rules("add", { required: true, messages: { required: "Last Name is required." } });
                    $("#Appraisal_Client2Person_Phone").rules("add", { required: true, messages: { required: "Phone is required." } });
                    $("#Appraisal_Client2Address_Address1").rules("add", { required: true, messages: { required: "Address is required." } });
                    $("#Appraisal_Client2Address_City").rules("add", { required: true, messages: { required: "City is required." } });
                    $("#Appraisal_Client2Address_StateCode").rules("add", { required: true, messages: { required: "State is required." } });
                    $("#Appraisal_Client2Address_PostalCode").rules("add", { required: true, messages: { required: "Zip Code is required." } });

                } else {
                    $("#OrderAppraisal #ClientInfo").hide("slow");

                    // Remove validation
                    removeValidation($("#Appraisal_Client2Person_FirstName"));
                    removeValidation($("#Appraisal_Client2Person_LastName"));
                    removeValidation($("#Appraisal_Client2Person_Phone"));
                    removeValidation($("#Appraisal_Client2Address_Address1"));
                    removeValidation($("#Appraisal_Client2Address_City"));
                    removeValidation($("#Appraisal_Client2Address_StateCode"));
                    removeValidation($("#Appraisal_Client2Address_PostalCode"));
                    // Remove validation messages
                    $("#ClientInfo span.field-validation-error").html("");
                    $("#ClientInfo span.field-validation-valid").html("");


                    $("#OrderAppraisal").validate();
                }
            });

        ///
        /// Remove validation rules from the form element
        ///
        var removeValidation = function (formElement) {
            // Remove validation rules
            formElement.rules("remove");
            formElement.removeClass("input-validation-error");
        };




        // autofill form
        $(document).keydown(function (e) {
            if (e.keyCode == 73 && e.ctrlKey) {

                //http://css-tricks.com/snippets/javascript/javascript-keycodes/

                //appraiserId:4
                //Appraisal.AppraiserId:4
                $("#Appraisal_ClientPerson_CompanyName").val("Ryan's Loan service");
                $("#Appraisal_ClientPerson_FirstName").val("Ryan");
                $("#Appraisal_ClientPerson_LastName").val("Loaner");
                $("#Appraisal_ClientPerson_Phone").val("(801) 555-1234");
                $("#Appraisal_ClientPerson_Email").val("ryanlifferth@gmail.com");
                $("#Appraisal_ClientAddress_Address1").val("123 Company Street");
                //#Appraisal_ClientAddress_Address2").val("");
                $("#Appraisal_ClientAddress_City").val("SLC");
                $("#Appraisal_ClientAddress_StateCode").val("UT");
                $("#Appraisal_ClientAddress_PostalCode").val("84121");
                $("#Appraisal_OccupantPerson_FirstName").val("Aimee");
                $("#Appraisal_OccupantPerson_LastName").val("Occupant");
                $("#Appraisal_OccupantPerson_Phone").val("(801) 555-4545");
                $("#Appraisal_OccupantPerson_Email").val("aimee@occupant.com");
                $("#Appraisal_PropertyTypeCode").val("SFR");
                $("#Appraisal_PropertyAddress_Address1").val("123 Property Place");
                //#Appraisal_PropertyAddress_Address2").val("");
                $("#Appraisal_PropertyAddress_City").val("Layton");
                $("#Appraisal_PropertyAddress_StateCode").val("UT");
                $("#Appraisal_PropertyAddress_PostalCode").val("84111");
                //Appraisal_ContactForAccess").val("false");
                $("#Appraisal_SalesContractPrice").val("355000");
                $("#Appraisal_LegalDescription").val("Legal description sample text.");
                $("#Appraisal_ReportUsers").val("Some company");
                $("#Appraisal_DeliverReportTo").val("Some company");
                $("#Appraisal_AppraisalPurposeCode").val("SC");
                $("#Appraisal_Comments").val("Please contact us at your earliest convenience.");

            }
        });
    });
