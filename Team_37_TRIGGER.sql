/*trigger to update review_count when a review is inserted*/
CREATE OR REPLACE FUNCTION update_count() RETURNS trigger AS '
BEGIN
    UPDATE business_table
    SET review_count=review_count+1
    WHERE business_id=new.business_id;
RETURN NULL;
END
' LANGUAGE plpgsql;

CREATE TRIGGER AddReviewCount
AFTER INSERT ON review_table
FOR EACH ROW
EXECUTE PROCEDURE update_count();

/*trigger to update review_rating when a review is inserted*/
CREATE OR REPLACE FUNCTION update_rating() RETURNS trigger AS '
BEGIN
    UPDATE business_table
    SET review_rating = (SELECT AVG(stars) FROM review_table)
    WHERE business_id=new.business_id;
RETURN NULL;
END
' LANGUAGE plpgsql;

CREATE TRIGGER AddReviewRating
AFTER INSERT ON review_table
FOR EACH ROW
EXECUTE PROCEDURE update_rating();

/*trigger to update num_checkins in business_table when a user checks in to a business*/
CREATE OR REPLACE FUNCTION update_checkins() RETURNS trigger AS '
BEGIN
    UPDATE business_table
    SET num_checkins = (SELECT SUM(checkin_table.count_var)
					FROM checkin_table
					WHERE checkin_table.business_id = new.business_id)
    WHERE business_id=new.business_id;
RETURN NULL;
END
' LANGUAGE plpgsql;

CREATE TRIGGER CheckIn
AFTER UPDATE OR INSERT ON checkin_table
FOR EACH ROW
EXECUTE PROCEDURE update_checkins();

/*tests for triggers*/

/*tests reviewadd triggers*/
INSERT INTO business_table
VALUES ('abcdef1234567890ghijkl', 'A business', 'WA', 'That one city', 64.7511, 147.3494, 0.0, TRUE, 99444, '123 Santa Lane', 0, 0, 0.0);

INSERT INTO user_table
VALUES('a1234567890bcdefghijkl', 'A Person', '2018-03-22', 0,0,0,0,0.0);

SELECT * FROM business_table
WHERE business_id='abcdef1234567890ghijkl';

INSERT INTO review_table
VALUES ('abcdefghijkl1234567890', 'a1234567890bcdefghijkl', 'abcdef1234567890ghijkl', '2018-03-22', 'This is a review of some kind', 3,0,0,0);

SELECT * FROM business_table
WHERE business_id = 'abcdef1234567890ghijkl';

DELETE FROM review_table
WHERE review_id = 'abcdefghijkl1234567890';

DELETE FROM user_table
WHERE user_id = 'a1234567890bcdefghijkl';

DELETE FROM business_table
WHERE business_id = 'abcdef1234567890ghijkl';
/*end review add trigger tests

/*test checkin trigger*/
INSERT INTO business_table
VALUES ('abcdef1234567890ghijkl', 'A business', 'WA', 'That one city', 64.7511, 147.3494, 0.0, TRUE, 99444, '123 Santa Lane', 0, 0, 0.0);

INSERT INTO checkin_table
VALUES ('abcdef1234567890ghijkl', 'Wednesday', '11:00', 1);

SELECT * FROM business_table
WHERE business_id = 'abcdef1234567890ghijkl';

SELECT * FROM checkin_table
WHERE business_id = 'abcdef1234567890ghijkl';

UPDATE checkin_table
SET count_var=count_var+1
WHERE business_id = 'abcdef1234567890ghijkl' AND day_var='Wednesday' AND hour_var='11:00';

SELECT * FROM business_table
WHERE business_id = 'abcdef1234567890ghijkl';

DELETE FROM checkin_table
WHERE business_id = 'abcdef1234567890ghijkl';

DELETE FROM business_table
WHERE business_id = 'abcdef1234567890ghijkl';
/*END TESTS*/