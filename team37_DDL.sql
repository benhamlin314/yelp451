
CREATE TABLE business_table(
	business_id CHAR(22),
	name VARCHAR(255) NOT NULL,
	state_var CHAR(2) NOT NULL,
	city VARCHAR(255) NOT NULL,
	latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL,
	stars FLOAT,
	is_open BOOLEAN DEFAULT TRUE,
	postal_code CHAR(5) NOT NULL,
	address VARCHAR(255) NOT NULL,

	review_count INT DEFAULT 0,	
	num_checkins INT DEFAULT 0 NOT NULL,
	review_rating FLOAT DEFAULT 0.0,
	
	PRIMARY KEY (business_id)
);

CREATE TABLE business_category_table(
	business_id CHAR(22),
	category VARCHAR(255),
	
	PRIMARY KEY (business_id, category),
	FOREIGN KEY (business_id) REFERENCES business_table(business_id)
);

CREATE TABLE business_attribute_table(
	business_id CHAR(22),
	attribute_name VARCHAR(255),
	val VARCHAR(500) NOT NULL,
	
	PRIMARY KEY (business_id, attribute_name),
	FOREIGN KEY (business_id) REFERENCES business_table(business_id)
);
	
CREATE TABLE user_table(
	user_id CHAR(22),
	name VARCHAR(255) NOT NULL,
	yelping_since CHAR(10), /*four digit year, dash, two digit month, dash, two digit day*/
	fans INT DEFAULT 0,
	funny INT DEFAULT 0,
	cool INT DEFAULT 0,
	useful INT DEFAULT 0,
	average_stars FLOAT,

	PRIMARY KEY (user_id)
);

CREATE TABLE review_table(
	review_id CHAR(22),
	user_id VARCHAR(255), /*NOT NULL ? */
	business_id VARCHAR(255) NOT NULL,
	
	date CHAR(10), /*four digit year, dash, two digit month, dash, two digit day*/
	text VARCHAR(5000), /* note: character limit is apparently 5000. */
	
	stars INT NOT NULL,
	funny INT DEFAULT 0,
	cool INT DEFAULT 0,
	useful INT DEFAULT 0,
	
	PRIMARY KEY (review_id),
	FOREIGN KEY (user_id) REFERENCES user_table(user_id),
	FOREIGN KEY (business_id) REFERENCES business_table(business_id)
	
);

CREATE TABLE checkin_table(
	business_id CHAR(22),
	day_var VARCHAR(255),
	hour_var VARCHAR(255), /* individual hour, or aggregated to the four time values from Milestone 1?) */
	count_var INT,
	
	PRIMARY KEY (business_id, day_var, hour_var),
	FOREIGN KEY (business_id) REFERENCES business_table(business_id)
);

CREATE TABLE hours_open_table(
	business_id CHAR(22),
	
	mondayOpen INT,
	mondayClose INT,
	tuesdayOpen INT,
	tuesdayClose INT,
	wednesdayOpen INT,
	wednesdayClose INT,
	thursdayOpen INT,
	thursdayClose INT,
	fridayOpen INT,
	fridayClose INT,
	saturdayOpen INT,
	saturdayClose INT,
	sundayOpen INT,
	sundayClose INT,
	
	PRIMARY KEY (business_id),
	FOREIGN KEY (business_id) REFERENCES business_table(business_id)
);

CREATE TABLE friendship_table(
	friend1_id CHAR(255),
	friend2_id CHAR(255),
	
	PRIMARY KEY (friend1_id, friend2_id),
	FOREIGN KEY (friend1_id) REFERENCES user_table(user_id),
	FOREIGN KEY (friend2_id) REFERENCES user_table(user_id)
);
