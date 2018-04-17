UPDATE business_table
SET num_checkins = (SELECT SUM(checkin_table.count_var)
					FROM checkin_table
					WHERE checkin_table.business_id = business_table.business_id);

UPDATE business_table
SET review_count = (SELECT COUNT(review_table.review_id)
					FROM review_table
					WHERE review_table.business_id = business_table.business_id);
					
UPDATE business_table
SET review_rating = (SELECT AVG(review_table.stars)
					FROM review_table
					WHERE review_table.business_id = business_table.business_id);
