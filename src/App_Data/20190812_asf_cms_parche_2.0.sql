USE asf_cms;

INSERT INTO menu VALUES("INFORMES");
INSERT INTO menu VALUES("ADMIN");
INSERT INTO menu VALUES("SECUNDARIO");
INSERT INTO menu VALUES("ACERCA");

ALTER TABLE menu ADD COLUMN css_class VARCHAR(128) NULL AFTER menu_key;
ALTER TABLE section_has_menu ADD COLUMN css_class VARCHAR(128) NULL AFTER position;

-- Post Alpha 1 (2019-08-30)

ALTER TABLE section ADD COLUMN type VARCHAR(64) NULL AFTER sitemap_exclude;

INSERT INTO section (created, updated, is_main, news_include, position, active, sitemap_exclude, redirect_options, type) VALUES(NOW(), NOW(), 0, 0, 0, 1, 1, '', 'eventos');
UPDATE section SET permalink = CONCAT(last_insert_id(), '_Eventos') WHERE id = last_insert_id();
INSERT INTO section_label VALUES(last_insert_id(), 1, 'Eventos');
INSERT INTO section (created, updated, is_main, news_include, position, active, sitemap_exclude, redirect_options, type) VALUES(NOW(), NOW(), 0, 0, 0, 1, 1, '', 'columnas');
UPDATE section SET permalink = CONCAT(last_insert_id(), '_Columnas') WHERE id = last_insert_id();
INSERT INTO section_label VALUES(last_insert_id(), 1, 'Columnas de opinión');

-- Post Beta 1 (2019-09-13)

ALTER TABLE section ADD COLUMN css_class VARCHAR(128) NULL AFTER type;
ALTER TABLE publication ADD COLUMN css_class VARCHAR(128) NULL AFTER sitemap_exclude;

ALTER TABLE file ADD COLUMN is_main INT(1) NOT NULL DEFAULT 0 AFTER title;

-- Final

CREATE TABLE IF NOT EXISTS modification_log (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	target_type VARCHAR(25) NOT NULL, -- section, publication
	target_id INT NOT NULL,
	username vARCHAR(30) NOT NULL,
	created DATETIME NOT NULL,
	modification_type VARCHAR(25) NOT NULL, -- create | modify | delete
	permalink VARCHAR(512),
    field_changes LONGTEXT,
    historic_id INT DEFAULT 0
    
) Engine = InnoDB;