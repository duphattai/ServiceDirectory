
//Tab Detail 1
function openTab() {
    var sub = document.getElementById("SubType");

    if (sub.options[sub.selectedIndex].value == "Contract") {
        $('#contractid').remove();
        $('#fundingid').remove();
        $('#ServiceTab').append('<li id="contractid"><a href="#contract" data-toggle="tab">Contract</a></li>');
    }
    else if (sub.options[sub.selectedIndex].value == "Independently Funded") {
        $('#contractid').remove();
        $('#fundingid').remove();
        $('#ServiceTab').append('<li id="fundingid"><a href="#funding" data-toggle="tab">Funding</a></li>');
    }

    //$('#contractid').hide();
    //$('#fundingid').hide();

    //if (sub.options[sub.selectedIndex].value == "Contract")
    //    $('#contractid').show();
    //else
    //    $('#fundingid').show();
}

function EnableExtendable(checkbox) {
    if (checkbox.checked) {
        document.getElementById("ExtendableYears").disabled = false;
        document.getElementById("ExtendableMonths").disabled = false;
    }
    else {
        document.getElementById("ExtendableYears").disabled = true;
        document.getElementById("ExtendableYears").value = 0;
        document.getElementById("ExtendableMonths").disabled = true;
        document.getElementById("ExtendableMonths").value = 0;
    }
}

function EnableTimeLimited(checkbox) {
    if (checkbox.checked) {
        document.getElementById("TimeLimitedYears").disabled = false;
        document.getElementById("TimeLimitedMonths").disabled = false;
    }
    else {
        document.getElementById("TimeLimitedYears").disabled = true;
        document.getElementById("TimeLimitedYears").value = 0;
        document.getElementById("TimeLimitedMonths").disabled = true;
        document.getElementById("TimeLimitedMonths").value = 0;
    }
}

//Tab Funding
function EnableContinuation(checkbox) {
    if (checkbox.checked) {
        document.getElementById("ContinuationAmount").disabled = false;
        document.getElementById("ContinuationDetails").disabled = false;
    }
    else {
        document.getElementById("ContinuationAmount").disabled = true;
        document.getElementById("ContinuationAmount").value = "";
        document.getElementById("ContinuationDetails").disabled = true;
        document.getElementById("ContinuationDetails").value = "";
    }
}