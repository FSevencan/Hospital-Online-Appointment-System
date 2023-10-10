
$(document).ready(function () {
    const $departmanSecimi = $("#DepartmanSecimi");
    const $doktorSecimi = $("#DoktorSecimi");
    const $datepicker = $("#datepicker");
    const $time = $('#time');

    // Departman değiştiğinde doktorları getir
    $departmanSecimi.change(function () {
        let departmanId = $(this).val();
        if (departmanId) {
            $.ajax({
                type: "POST",
                url: "/Randevu/GetDoktorlar",
                data: { departmanID: departmanId },
                success: function (response) {
                    $doktorSecimi.empty().append($('<option>', { value: "", text: "Doktor Seçin" }));
                    if (response.length > 0) {
                        $.each(response, function (i, item) {
                            $doktorSecimi.append($('<option>', {
                                value: item.doktorID,
                                text: item.adSoyad
                            }));
                        });
                    } else {
                        $doktorSecimi.append($('<option>', {
                            value: "",
                            text: "*Doktor bulunamadı"
                        }));
                    }
                }
            });
        } else {
            $doktorSecimi.empty().append($('<option>', {
                value: "",
                text: "*Önce Tıbbi Birim Seçin"
            }));
        }
    });

    // Doktor seçimi değiştiğinde tarih ve saatleri sıfırla
    $doktorSecimi.change(function () {
        $datepicker.val('Tarih seçin');
        $time.text('');

        let doktorId = $(this).val();
        if (doktorId) {
            $.ajax({
                type: "POST",
                url: "/Randevu/GetDoktorTarihleri",
                data: { doktorId: doktorId },
                success: function (response) {
                    if (response.success) {
                        let availableDates = response.tarihler.map(t => new Date(t));

                        $datepicker.datepicker("option", "beforeShowDay", function (date) {
                            if (availableDates.some(availableDate => availableDate.getTime() === date.getTime())) {
                                return [true, ""];
                            }
                            return [false, ""];
                        });
                    } else {
                        $datepicker.datepicker("option", "beforeShowDay", function (date) {
                            return [false, ""];
                        });
                    }
                }
            });
        } else {
            $datepicker.datepicker("option", "beforeShowDay", function (date) {
                return [false, ""];
            });
        }
    });

    $(".date-picker-icon").click(function () {
        $("#datepicker").datepicker("show");
    });

    // Tarih seçimi değiştiğinde saatleri güncelle
    $datepicker.datepicker({
        monthNames: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
        dayNamesMin: ["Pzr", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
        dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
        firstDay: 1,
        dateFormat: "dd.mm.yy",
        minDate: +1,
        maxDate: "+1W",
        onSelect: function (dateText, inst) {
            let selectedDoctor = $doktorSecimi.val();
            if (selectedDoctor) {
                updateAvailableHours();
            }
        }
    });

    function updateAvailableHours() {
        var doktorId = $doktorSecimi.val();
        var selectedDate = $datepicker.val();
        if (doktorId && selectedDate) {
            $.ajax({
                type: "POST",
                url: "/Randevu/GetDoktorSaati",
                data: { doktorId: doktorId, tarih: selectedDate },
                success: function (data) {
                    $time.empty();
                    if (data.success) {
                        $.each(data.saatler, function (key, value) {
                            if (value) {
                                $time.append('<option value="' + key + '">' + key + ':00</option>');
                            }
                        });
                        if (!$time.children().length) {
                            $time.html('<option value="">' + "*Tüm randevu saatleri dolu" + '</option>');
                        }
                    } else {
                        $time.html('<option value="">' + data.message + '</option>');
                    }
                },
            });
        }
    }

    // Form submit işlemini engelle ve ajax ile gönder
    $('#appointment-form').on('submit', function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Randevunuz başarıyla oluşturuldu!',
                    showConfirmButton: false,
                    timer: 3000
                });
                $('#appointment-form')[0].reset();
                $time.text('');
            }
        });
    });
});