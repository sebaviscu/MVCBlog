function OnlyInteger(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
}

function OnlyDecimal(e) {
    if (e.which != 8 && e.which != 0 && e.which != 44 && e.which != 46 && (e.which < 48 || e.which > 57)) {
        return false;
    }
    else if (e.which == 44 || e.which == 46) {
        var str = $(e)[0].currentTarget.value;
        if (str.indexOf(',') != -1) {
            return false;
        }
        else {
            $(e)[0].currentTarget.value = str + ',';
            e.preventDefault();
        }
    }
}