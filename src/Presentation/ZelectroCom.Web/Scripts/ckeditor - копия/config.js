/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' }
	];

	// Set the most common block elements.
	config.format_tags = 'p;h2;h3';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';
	
	config.image_previewText = " ";

	config.codeSnippet_theme = 'github-gist';

	config.codeSnippet_languages = {
	    cpp: 'Arduino Wiring / C++',
	    javascript: 'JavaScript',
	    php: 'PHP',
        html: 'html',
	    csharp: 'C#',
	    java: 'Java'
	};
};

CKEDITOR.on('dialogDefinition', function (ev) {
    // Take the dialog name and its definition from the event data.
    var dialogName = ev.data.name;
    var dialogDefinition = ev.data.definition;

    // Check if the definition is from the dialog we're
    // interested in (the 'link' dialog).
    if (dialogName == 'link') {
        // Get a reference to the 'Link Info' tab.
        var infoTab = dialogDefinition.getContents('info');
        infoTab.remove('protocol');
    }
});