// Films Management System JavaScript

// Auto-hide alerts after 5 seconds
$(document).ready(function () {
    setTimeout(function () {
        $('.alert:not(.alert-permanent)').fadeOut('slow');
    }, 5000);
});

// Confirm delete actions
function confirmDelete(entityName, entityType) {
    return confirm(`Are you sure you want to delete this ${entityType}?\n\nName: ${entityName}\n\nThis action cannot be undone.`);
}

// Search functionality
function performSearch(searchTerm) {
    if (searchTerm.length < 2) {
        return;
    }

    // Add loading indicator
    const searchButton = document.querySelector('button[type="submit"]');
    if (searchButton) {
        searchButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Searching...';
        searchButton.disabled = true;
    }
}

// Form validation helpers
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        return form.checkValidity();
    }
    return false;
}

// Tooltip initialization
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// Table row highlighting
$(document).ready(function () {
    $('.table tbody tr').hover(
        function () {
            $(this).addClass('table-active');
        },
        function () {
            $(this).removeClass('table-active');
        }
    );
});

// Auto-focus first input field on page load
$(document).ready(function () {
    $('input[type="text"]:first, textarea:first').focus();
});