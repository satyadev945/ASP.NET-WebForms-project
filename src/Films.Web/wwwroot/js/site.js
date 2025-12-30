// Site-wide JavaScript for Films Application

// Initialize tooltips
$(document).ready(function () {
    // Initialize Bootstrap tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Auto-hide alerts after 5 seconds
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 5000);

    // Confirm delete actions
    $('.btn-outline-danger[href*="/Delete"]').click(function (e) {
        if (!confirm('Are you sure you want to delete this item?')) {
            e.preventDefault();
        }
    });

    // Form validation feedback
    $('form').submit(function () {
        var isValid = $(this)[0].checkValidity();
        if (!isValid) {
            $(this).addClass('was-validated');
        }
    });

    // Image preview for URL inputs
    $('input[type="url"][id*="ImageUrl"]').on('input', function () {
        var url = $(this).val();
        var previewId = 'preview-' + $(this).attr('id');

        // Remove existing preview
        $('#' + previewId).remove();

        if (url && isValidUrl(url)) {
            var img = $('<img>')
                .attr('id', previewId)
                .attr('src', url)
                .addClass('img-thumbnail mt-2')
                .css({ 'max-width': '200px', 'max-height': '150px' })
                .on('error', function () {
                    $(this).remove();
                });

            $(this).parent().append(img);
        }
    });

    // Search form auto-submit on Enter
    $('input[name="SearchTerm"]').keypress(function (e) {
        if (e.which === 13) { // Enter key
            $(this).closest('form').submit();
        }
    });
});

// Utility functions
function isValidUrl(string) {
    try {
        new URL(string);
        return true;
    } catch (_) {
        return false;
    }
}

// Show loading spinner for form submissions
function showLoadingSpinner(button) {
    var originalText = button.innerHTML;
    button.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...';
    button.disabled = true;

    // Re-enable after 10 seconds as fallback
    setTimeout(function () {
        button.innerHTML = originalText;
        button.disabled = false;
    }, 10000);
}

// Format dates consistently
function formatDate(dateString) {
    if (!dateString) return '';

    var date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

// Debounce function for search inputs
function debounce(func, wait, immediate) {
    var timeout;
    return function executedFunction() {
        var context = this;
        var args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
}