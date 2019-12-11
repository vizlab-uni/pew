from datetime import datetime
import time

print("start")

time.sleep(7) 
now = datetime.now()
mm = str(now.month)
dd = str(now.day)
yyyy = str(now.year)
hour = str(now.hour)
mi = str(now.minute)
ss = str(now.second)


print(mm + "/" + dd + "/" + yyyy + " " + hour + ":" + mi + ":" + ss)
print("end")