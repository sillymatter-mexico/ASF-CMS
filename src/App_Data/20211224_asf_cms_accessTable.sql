CREATE TABLE `publication_link_access` (
	`access_date` DATE NOT NULL,
	`access_url` VARCHAR(500) NOT NULL,
	`hit_count` INT(10) UNSIGNED NOT NULL,
	PRIMARY KEY (`access_date`, `access_url`) USING BTREE,
	INDEX `Index_Url` (`access_url`)
)
DEFAULT CHARSET=latin1
ENGINE=MyISAM